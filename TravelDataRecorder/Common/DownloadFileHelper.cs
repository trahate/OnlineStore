using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TravelDataRecorder.Common
{
    public class DownloadFileHelper
    {
        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="filePath">Full qualified path of the file including file name </param>
        public static void DownloadFile(string filePath)
        {
            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            //HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            //HttpContext.Current.Response.ContentType = "application/pdf";
            //HttpContext.Current.Response.TransmitFile(file.FullName);
            //HttpContext.Current.Response.End();
            //string filepath = Server.MapPath("../upload_files/CDMS_Kit_Order_Nomenclature_R5.pdf");

            FileInfo file = new FileInfo(filePath);
            // Checking if file exists
            if (file.Exists)
            {
                // Clear the content of the response
                HttpContext.Current.Response.ClearContent();

                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                // Add the file size into the response header
                HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

                // Set the ContentType
                HttpContext.Current.Response.ContentType = file.Extension.ToLower();

                // Write the file into the response 
                HttpContext.Current.Response.TransmitFile(file.FullName);
                // End the response
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// Create a folder if not exists under root directory. 
        /// Also creates root directory if not exists
        /// </summary>
        /// <param name="rootFolder">Folder name under which sub folder/ child folder to be created</param>
        /// <param name="folderName">Name of the folder taht is to be created under root directory</param>
        public static void CreateFolder(string rootFolder, string folderName)
        {
            string rootDirectoryPath = HttpContext.Current.Server.MapPath("~\\" + rootFolder);
            if (!Directory.Exists(rootDirectoryPath))
            {
                Directory.CreateDirectory(rootDirectoryPath);
            }
            string folderPath = rootDirectoryPath + "\\" + folderName;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        public static void DeleteOldExcelFiles(ExcelExportTypeEnum whichFiles)
        {
            string directoryPath = HttpContext.Current.Server.MapPath("~\\ReportData");
            string directoryName = string.Empty;
            switch (whichFiles)
            {
                case ExcelExportTypeEnum.ReportFilters:
                    directoryName = "\\Excel";
                    break;

            }

            string fileToDeleteDirectory = directoryPath + directoryName;
            if (System.IO.Directory.Exists(fileToDeleteDirectory))
            {
               var oldFiles = Directory.EnumerateFiles(fileToDeleteDirectory).Where(x => new FileInfo(x).CreationTime.Date < DateTime.Today.Date);
                foreach (string filePath in oldFiles)
                {
                    File.Delete(filePath);
                }
            }
        }
        public enum ExcelExportTypeEnum
        {
            ReportFilters = 1,
        }
        }
}