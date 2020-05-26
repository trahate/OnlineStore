using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TravelDataRecorder.APIControllers;
using TravelDataRecorder.Common;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using Newtonsoft.Json;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;
namespace TravelDataRecorder.Controllers
{
    public class HomeController : TravelBaseController
    {
        //public ActionResult Index()
        //{
        //    ViewBag.Title = "Home Page";

        //    return View();
        //}
        ManageUserAPIController objManageUser;

        public HomeController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {
            objManageUser = new ManageUserAPIController(_userRepository);

        }
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var asd = objManageUser.GetAllUser();
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserModel objUserModel)
        {
            UserModel objUser = objManageUser.GetUserByEmail(objUserModel.Email);
            if (objUser == null)
            {
                ViewBag.IsOTPSent = false;
                ViewBag.Message = "Email address does not exist";
                return View(objUserModel);
            }
            else if (objUser != null && string.IsNullOrEmpty(objUser.Password))
            {
                ViewBag.IsOTPSent = false;
                ViewBag.Message = "Email address does not exist";
                return View(objUserModel);
            }

            if (string.IsNullOrEmpty(objUserModel.OTP))
            {
                //generate OTP
                Random random = new Random();
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string OTP = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
                objUser.OTP = OTP;
                //generate OTP

                //to address
                Dictionary<int, string> toAddress = new Dictionary<int, string>();
                toAddress.Add(0, objUserModel.Email);
                //to address

                //send email
                EmailHelper objEmailHelper = new EmailHelper(_dbErrorHandlingService);
                //objEmailHelper.SendMail(toAddress, null, null, "Hi your OTP is " + OTP, "Forgot Password OTP");
                EmailHelper.SendMail(toAddress, null, null, "Hi your OTP is " + OTP, "Forgot Password OTP");
                //send email

                //update DB
                objManageUser.UpdateUser(objUser);
                //update DB

                //show message to user
                ViewBag.IsOTPSent = true;
                ViewBag.Message = "An OTP have been sent to your email address";
                //show message to user
                objUser.OTP = "";

                return View(objUser);
            }
            else
            {
                if (objUser.OTP != objUserModel.OTP)
                {
                    ViewBag.IsOTPSent = true;
                    ViewBag.Message = "OTP does not match";
                    return View(objUserModel);
                }
                TempData["userId"] = objUser.ID;
                return RedirectToAction("generatepassword", "home");
            }
        }
        [HttpGet]
        public ActionResult LogIn()
        {
            if (TempData["PasswordChanged"] != null)
            {
                ViewBag.Message = TempData["PasswordChanged"].ToString();
            }
            LogInModel data = new LogInModel();
            return View(data);
        }


        public ActionResult GeneratePassword(Guid? Key)
        {
            int? userId = null;
            if (TempData["userId"] != null)
            {
                userId = Convert.ToInt32(TempData["userId"]);
            }
            TravelUserPasswordViewModel Addpassword = new TravelUserPasswordViewModel();
            UserModel user = new UserModel();
            if (userId != null)
            {
                user = objManageUser.GetUserById(userId.Value);
            }
            else
            {
                user = objManageUser.GetUserByKey(Key.Value);
            }

            if (user != null && (string.IsNullOrEmpty(user.Password) || Key == null))
            {
                Addpassword.Id = user.ID;
                return View(Addpassword);
            }
            else
            {
                ViewBag.IsExpired = true;
                return View(new TravelUserPasswordViewModel());
            }
        }

        [HttpPost]
        public ActionResult GeneratePassword(TravelUserPasswordViewModel passwordVM)
        {

            if (ModelState.IsValid)
            {
                UserModel User = objManageUser.GetUserById(passwordVM.Id);
                User.Password = EncryptDecrypt.Encrypt(passwordVM.ConfirmPassword); // encrypt password in EncryptDecrypt in common 
                objManageUser.UpdateUser(User);
                ViewBag.Message = "Your Password has been generated successfully";

                TempData["PasswordChanged"] = "Your Password has been generated successfully";
                return RedirectToAction("login", "home");
            }
            else
            {
                ViewBag.Message = String.Format("Password and Confirm Password Does Not Match");
                return View(passwordVM);
            }

            // return View(passwordVM);
        }

        [HttpPost]
        public ActionResult LogIn(LogInModel UserLogin)
        {
            if (ModelState.IsValid)
            {
                UserLogin.Password = EncryptDecrypt.Encrypt(UserLogin.Password);
                UserModel UserData = objManageUser.LogIn(UserLogin);
                if (UserData != null)
                {
                    Session.Abandon();
                    CustomUserModel CustomUser = new CustomUserModel();
                    var serializer = new JavaScriptSerializer();
                    CustomUser.UserName = UserData.Email;
                    CustomUser.ID = UserData.ID;
                    CustomUser.FullName = UserData.FirstName + " " + UserData.LastName;
                    if (UserData.TravelUserRoleMappings[0].RoleID != null)
                    {
                        CustomUser.RoleId = UserData.TravelUserRoleMappings[0].RoleID;
                        CustomUser.RoleName = UserData.TravelUserRoleMappings[0].TravelRole.Name;
                    }

                    System.Web.HttpContext.Current.Items.Add("User", CustomUser);
                    string userData = serializer.Serialize(CustomUser);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1,
                    CustomUser.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(10080),
                    true,
                    userData);
                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    ///Not in use for now but will be used in api for validating/passing token
                    TokenData tokenData = JsonConvert.DeserializeObject<TokenData>(GetAPIToken(UserData.Email, UserData.Password));
                    CustomUser.access_token = tokenData.access_token;
                    CustomUser.token_type = tokenData.token_type;
                    CustomUser.expires_in = tokenData.expires_in;

                    //return RedirectToAction("profile", "user");
                    return RedirectToAction("dashboard", "dashboard");
                }
                else
                {
                    ViewBag.Message = String.Format("Invalid Email or Password. Please try again");
                    return View(UserLogin);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Data is not valid");
                return View(UserLogin);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            // System.Web.HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            string[] cookiesToRemove = new[] { FormsAuthentication.FormsCookieName, FormsAuthentication.FormsCookieName };

            foreach (var cookieName in cookiesToRemove)
            {
                if (Request.Cookies[cookieName] != null)
                {
                    HttpCookie curCookie = new HttpCookie(cookieName);
                    curCookie.Expires = DateTime.Now.AddDays(-2);
                    Response.Cookies.Add(curCookie);
                }
            }

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("login");
        }

        //public ActionResult Error()
        //{
        //    return View();
        //}
        #region Helper methods
        private string GetAPIToken(string userName, string password)
        {
            string requestUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/token";
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            var postData = "username=" + userName;
            postData += "&password=" + password;
            postData += "&grant_type=password";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }
        #endregion
    }
}
