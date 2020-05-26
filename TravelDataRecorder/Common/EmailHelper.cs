using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using TravelDataRecorder.Model;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.Common
{
    public class EmailHelper
    {

        public static IDbErrorHandlingService _IDbErrorHandlingService;

        public EmailHelper(IDbErrorHandlingService IDbErrorHandlingService)
        {
           _IDbErrorHandlingService = IDbErrorHandlingService;
        }


        /// <summary>
        /// Send Mail Function
        /// </summary>
        /// <param name="toAddresses"></param>
        /// <param name="ccAddresses"></param>
        /// <param name="attachmentFiles"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        public static bool SendMail(Dictionary<int, string> toAddresses, Dictionary<int, string> ccAddresses, Dictionary<int, string> attachmentFiles, string message, string subject)
        {
            try
            {
                if (ConfigurationManager.AppSettings["EnableMailing"].ToString() == "0")
                {
                    return false;
                }
                System.Net.Mail.MailMessage mailMessageObject = new System.Net.Mail.MailMessage();
                mailMessageObject.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient();
                string EmailUser = ConfigurationManager.AppSettings["EmailUser"].ToString();
                string EmailPass = ConfigurationManager.AppSettings["EmailPass"].ToString();
                smtp.Credentials = new NetworkCredential(EmailUser, EmailPass);
                MailAddress mailFrom = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"].ToString(), "Travel Data Support Team");
                string strSmtpServer = ConfigurationManager.AppSettings["SMTP"].ToString();
                smtp.Host = strSmtpServer;
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                mailMessageObject.From = mailFrom;

                if (attachmentFiles != null)
                {
                    foreach (var item in attachmentFiles)
                    {
                        mailMessageObject.Attachments.Add(new Attachment(item.Value));
                    }
                }
                mailMessageObject.Subject = subject;
                mailMessageObject.Body = message;
                mailMessageObject.IsBodyHtml = true;
                //for (int i = 0; i < ccAddresses.Count; i++)
                //{
                //    mailMessageObject.CC.Add(new MailAddress(ccAddresses[i]));
                //}

                if (ccAddresses != null)
                {
                    foreach (var item in ccAddresses)
                    {
                        mailMessageObject.CC.Add(new MailAddress(item.Value));
                    }
                }

                if (toAddresses.Count > 0)
                {
                    //for (int i = 0; i < toAddresses.Count; i++)
                    //{
                    //    mailMessageObject.To.Add(new MailAddress(toAddresses[i]));
                    //}

                    foreach (var item in toAddresses)
                    {
                        mailMessageObject.To.Add(new MailAddress(item.Value));
                    }
                    try
                    {
                        bool enableSsl = ConfigurationManager.AppSettings["SMTPEnableSsl"].ToString() == "0" ? false : true;
                        smtp.EnableSsl = enableSsl;
                        smtp.Send(mailMessageObject);
                        mailMessageObject.Dispose();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        mailMessageObject.Dispose();
                        //SaveExceptionToDatabase(toAddresses, ccAddresses, message, subject, ex);
                        return false;

                        //string queName = ConfigurationManager.AppSettings["Queue"];
                        //MSMQ msg = new MSMQ(queName);
                        //msg.QueueMessage(m);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveExceptionToDatabase(toAddresses, ccAddresses, message, subject, ex);
            }
            return false;
        }

        private static void SaveExceptionToDatabase(Dictionary<int, string> toAddresses, Dictionary<int, string> ccAddresses, string message, string subject, Exception ex)
        {
            string commaSepratedToAddress = string.Empty;
            foreach (var item in toAddresses)
            {
                commaSepratedToAddress += item.Value + ",";
            }
            commaSepratedToAddress = commaSepratedToAddress.TrimEnd(',');
            string commaSepratedCCAddress = string.Empty;
            if (ccAddresses != null)
            {
                foreach (var item in ccAddresses)
                {
                    commaSepratedCCAddress += item.Value + ",";
                }
            }
            commaSepratedCCAddress = commaSepratedCCAddress.TrimEnd(',');

            EmailExceptionModel objemailexception = new EmailExceptionModel()
            {
                ErrorInnerException = Convert.ToString(ex.InnerException),
                ErrorMessage = ex.Message,
                ErrorInnerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "",
                MailMessage = message,
                MailSubject = subject,
                MailTo = commaSepratedToAddress,
                MailCC = commaSepratedCCAddress,
            };
            //save to database when exception occurred
            _IDbErrorHandlingService.AddEmailException(objemailexception);
            //AppScope.ErrorHandlingServiceInstance.AddEmailException(objemailexception);
        }

        /// <summary>
        /// Send Mail with Bcc
        /// </summary>
        /// <param name="toAddresses"></param>
        /// <param name="ccAddresses"></param>
        /// <param name="bccAddresses"></param>
        /// <param name="attachmentFiles"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        public void SendMailwithBcc(List<string> toAddresses, List<string> ccAddresses, List<string> bccAddresses, List<string> attachmentFiles, string message, string subject)
        {
            #region Remove Duplicates Emails
            toAddresses = RemoveDuplicateEmails(toAddresses);
            ccAddresses = RemoveDuplicateEmails(ccAddresses);
            ccAddresses = RemoveDuplicateEmails(toAddresses, ccAddresses);
            #endregion

            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
            m.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            string EmailUser = ConfigurationManager.AppSettings["EmailUser"].ToString();
            string EmailPass = ConfigurationManager.AppSettings["EmailPass"].ToString();
            smtp.Credentials = new NetworkCredential(EmailUser, EmailPass);
            MailAddress mailFrom = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"].ToString(), "TMS");
            string strSmtpServer = ConfigurationManager.AppSettings["SMTP"].ToString();
            smtp.Host = strSmtpServer;
            m.From = mailFrom;

            //AlternateView av = AlternateView.CreateAlternateViewFromString(message,
            //                     null, MediaTypeNames.Text.Html);

            //string pathImg1 = HttpContext.Current.Server.MapPath("/images/img1.png");
            //string pathImg2 = HttpContext.Current.Server.MapPath("/images/img2.png");
            //string pathImg3 = HttpContext.Current.Server.MapPath("/images/img3.png");

            //LinkedResource lr = new LinkedResource(pathImg1, MediaTypeNames.Image.Jpeg);
            //lr.ContentId = "image1";
            //av.LinkedResources.Add(lr);


            //lr = new LinkedResource(pathImg2, MediaTypeNames.Image.Jpeg);
            //lr.ContentId = "image2";
            //av.LinkedResources.Add(lr);

            //lr = new LinkedResource(pathImg3, MediaTypeNames.Image.Jpeg);
            //lr.ContentId = "image3";

            //av.LinkedResources.Add(lr);
            // m.AlternateViews.Add(av);

            m.Subject = subject;
            m.Body = message;
            m.IsBodyHtml = true;

            for (int i = 0; i < attachmentFiles.Count; i++)
            {
                m.Attachments.Add(new Attachment(attachmentFiles[i]));
            }

            for (int i = 0; i < ccAddresses.Count; i++)
            {
                m.CC.Add(new MailAddress(ccAddresses[i]));
            }
            for (int bccCount = 0; bccCount < bccAddresses.Count; bccCount++)
            {
                m.Bcc.Add(new MailAddress(bccAddresses[bccCount]));
            }
            if (toAddresses.Count > 0)
            {
                for (int i = 0; i < toAddresses.Count; i++)
                    m.To.Add(new MailAddress(toAddresses[i]));
                try
                {
                    smtp.Send(m);
                }
                catch (Exception ex)
                {
                    string queName = ConfigurationManager.AppSettings["Queue"];
                    //MSMQ msg = new MSMQ(queName);
                    //msg.QueueMessage(m);
                }
            }

        }


        /// <summary>
        /// Generic function for Remove Duplicate Emails  
        /// </summary>
        /// <param name="lstEmails"></param>
        /// <returns></returns>
        public List<string> RemoveDuplicateEmails(List<string> lstEmails)
        {
            List<string> lstDistinctEmails = new List<string>();
            foreach (string s in lstEmails)
            {
                if (!lstDistinctEmails.Contains(s))
                {
                    lstDistinctEmails.Add(s);
                }
            }
            return lstDistinctEmails;
        }

        /// <summary>
        /// Generic function for Remove Duplicate Emails  
        /// </summary>
        /// <param name="lstEmailsTo"></param>
        /// <param name="lstEmailsCC"></param>
        /// <returns></returns>
        public List<string> RemoveDuplicateEmails(List<string> lstEmailsTo, List<string> lstEmailsCC)
        {
            List<string> lstDistinctEmails = new List<string>();

            foreach (string s in lstEmailsCC)
            {
                if (!lstEmailsTo.Contains(s))
                {
                    lstDistinctEmails.Add(s);
                }
            }
            return lstDistinctEmails;
        }

        public static void AddErrorLog(ErrorLogModel objErrorLogModel)
        {
            _IDbErrorHandlingService.AddErrorLog(objErrorLogModel);
        }
    }
}
