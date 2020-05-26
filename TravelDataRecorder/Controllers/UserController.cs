using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using TravelDataRecorder.Common;
using Newtonsoft.Json;
using System.Net;
using TravelDataRecorder.Common.TravelDataEnum;

namespace TravelDataRecorder.Controllers
{
    [Authorize]
    public class UserController : TravelBaseController
    {
        public UserController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {
        }



        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<TravelDetailModel> traveldata = objmanageTravel.GetTravelDetailByUserId(CurrentUserDetail().ID).Where(x => x.Summary_SubmittedDate >= DateTime.Now.AddMonths(-6));
            DashboardViewModel dashboard = new DashboardViewModel();
            try
            {
                dashboard.Approved = traveldata.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
                dashboard.InProgress = traveldata.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1).Count();
                dashboard.Reject = traveldata.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager).Count();
                dashboard.TotalTravel = traveldata.Count();
            }
            catch
            {
            }

            return PartialView("_UserDashboard", dashboard);


        }

        [ChildActionOnly]
        public ActionResult ProfilePic()
        {
            UserModel User = objManageUser.GetUserById(CurrentUserDetail().ID);
            var path = Server.MapPath("~/ProfileImages/" + User.ProfileImage);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                ViewBag.Path = User.ProfileImage;
            }
            else
            {
                ViewBag.Path = "no-image.png";

            }

            return PartialView("_ProfilePic");
        }
        [HttpGet]
        public ActionResult Profile()
        {
            if (TempData["ProfileImageError"] != null)
            {
                ViewBag.Message1 = TempData["ProfileImageError"].ToString();
            }
            if (TempData["ProfileImageSaved"] != null)
            {
                ViewBag.Message = TempData["ProfileImageSaved"].ToString();
            }

            if (TempData["ProfileUpdate"]!=null)
            {
                ViewBag.Message = TempData["ProfileUpdate"].ToString();
            }
            UserModel User;
            int userid = 0;
            if (TempData["uid"] != null)
            {
                userid = Convert.ToInt32(TempData["uid"]);
                
                User = objManageUser.GetUserById(userid);
            }
            else
            {
                 User = objManageUser.GetUserById(CurrentUserDetail().ID);
                TempData["userid"] = null;
                TempData["edituser"] = null;
            }
            var path = Server.MapPath("~/ProfileImages/" + User.ProfileImage);
            FileInfo file = new FileInfo(path);
            if (file.Exists.Equals(false))
            {
                User.ProfileImage = "no-image.png";

            }


            return View(User);
        }
        public ActionResult viewprofile(int userid,string edituser)
        {
            TempData["userid"] = userid;
            TempData["edituser"] = edituser;

            return RedirectToAction("getprofilebyid");
        }

        public ActionResult GetProfileById()
        {
            int userid = 0;
            string edituser = "";
            if (TempData["userid"] != null && TempData["edituser"]!=null)
            {
                userid = Convert.ToInt32(TempData["userid"]);
                TempData["userid"] = userid;
                edituser =Convert.ToString(TempData["edituser"]);
                TempData["edituser"] = edituser;

            }
            UserModel User = objManageUser.GetUserById(userid);
            var path = Server.MapPath("../ProfileImages/" + User.ProfileImage);
            FileInfo file = new FileInfo(path);
            if (file.Exists.Equals(false))
            {
                User.ProfileImage = "no-image.png";

            }
          
            return View("profile", User);
        }

        [HttpPost]
        public ActionResult UpdateUser(UserModel Userobj)
        {

            Userobj.UserName = Userobj.Email;
            Userobj.IsActive = 1;
            if (ModelState.IsValid)
            {
                objManageUser.UpdateUser(Userobj);

                ViewBag.Message = "Account information has been updated successfully";

                TempData["ProfileUpdate"]= "Account information has been updated successfully";
                if (TempData["userid"] != null && TempData["edituser"] != null)
                {
                    TempData["uid"] = Userobj.ID;
                }
                return RedirectToAction("profile");
            }
            else
            {
                if (TempData["userid"] != null && TempData["edituser"] != null)
                {
                    TempData["uid"] = Userobj.ID;
                }
                return RedirectToAction("profile");
            }
        }

        public ActionResult DashBoard()
        {
            return View();
        }
      
        
        
        
        [ChildActionOnly]
        public ActionResult Notification()
        {
            UserModel User = objManageUser.GetUserById(CurrentUserDetail().ID);
            IEnumerable<TravelNotificationModel> notification = objmanageCommon.GetNotification(User.ID);
            return PartialView("_Notification", notification);
        }
        public ActionResult NotificationList()
        {

            SetNotificationList();
            return View();
        }

        //public ActionResult viewnotification()
        //{
        //    //IEnumerable<TravelNotificationModel> notificationlist=new 
        //    //UserModel User = objManageUser.GetUserById(CurrentUserDetail().ID);
        //    //IEnumerable<TravelNotificationModel> notificationlist = objmanageCommon.GetNotificationList(User.ID);
        //    //return View("notificationlist", notificationlist);
        //}

        public void SetNotificationList()
        {
            List<TravelDetailModel> lstRoles = new List<TravelDetailModel>();
            //var result = objmanageTravel.GetAllRole();
            //if (result.StatusCode == HttpStatusCode.OK)
            //{
            //    lstRoles = JsonConvert.DeserializeObject<List<TravelDetailModel>>(result.Content.ReadAsStringAsync().Result);
            //}
        }
        [HttpPost]
        public ActionResult GetNotificationByAjax(int draw, int start, int length, string status, string criteria, string customSearch, string isactive)
        {
            SearchFilter searchFilter = CommonHelper.GetSearchFilter();
            criteria = String.IsNullOrEmpty(criteria) ? searchFilter.Criteria : criteria;
            string search = Request.Form["search[value]"];
            if (String.IsNullOrEmpty(search) && !String.IsNullOrEmpty(customSearch))
            {
                search = customSearch;
            }

            // note: we only sort one column at a time
            int sortColumn = -1;
            string OrderColumnName = "";
            string sortColumnDir = "asc";
            if (Request.Form["order[0][column]"] != null)
            {
                sortColumn = int.Parse(Request.Form["order[0][column]"]);
                OrderColumnName = Request.Form["columns[" + sortColumn + "][data]"];
            }

            if (Request.Form["order[0][dir]"] != null)
            {
                sortColumnDir = Request.Form["order[0][dir]"];
            }

            if (Request.IsAjaxRequest())
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int recordFilteredTotal = 0;

                // Get complaint Records.   
                var lstNotification = GetNotificationData(status, criteria, searchFilter.FromDate.ToString(), searchFilter.ToDate.ToString(),isactive);

                // 1. Searching  
                if (!string.IsNullOrEmpty(search))
                {
                    lstNotification = lstNotification.Where(c => (c.Name != null && c.Name.ToLower().Contains(search.ToLower()))
                              ||
                             
                              (c.Message.ToString() != null && c.Message.ToString().ToLower().Contains(search.ToLower()))
                              ||
                              (c.NotificationDate != null && c.NotificationDate.ToString("MM/dd/yyyy").ToLower().Contains(search.ToLower()))).ToList();

                }

                // 2. Get the total record count  

                // 3. Sorting  



                var filteredData = lstNotification as IEnumerable<TravelNotificationModel>;
                Func<TravelNotificationModel, string> orderingFunction = (c => OrderColumnName == "Name" ? c.Name :
                                          OrderColumnName == "Message" ? c.Message.ToString() :
                                          OrderColumnName == "NotificationDate" ? c.NotificationDate.ToString()  : c.Name);
                if (sortColumnDir == "asc")
                    filteredData = filteredData.OrderBy(orderingFunction);
                else
                    filteredData = filteredData.OrderByDescending(orderingFunction);

                //switch (criteria)
                //{
                //    case "Approved":

                //        filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2);
                //        break;
                //    case "InProgress":
                //        if (CurrentUserDetail().RoleId == (int)UserRoleEnum.User)
                //        {
                //            filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1);
                //        }
                //        else
                //        {
                //            filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1);
                //        }
                //        break;
                //    case "Reject":
                //        filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager);
                //        break;
                //    case "New":
                //        DateTime Yesterday = DateTime.UtcNow.AddDays(-1);
                //        filteredData = filteredData.Where(x => x.Summary_SubmittedDate >= Yesterday);
                //        break;
                //    case "TotalTravel":
                //        filteredData = filteredData;
                //        break;
                //}
                // 4. Filtering  
                recordsTotal = filteredData.Count();
                filteredData = filteredData.Skip(skip).Take(pageSize).ToList();

                // 5. Get the filtered record count  
                recordFilteredTotal = filteredData.Count();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordFilteredTotal, data = filteredData }, JsonRequestBehavior.AllowGet);
            }


            return View();
        }
        private List<TravelNotificationModel> GetNotificationData(string Status, string criteria, string fromDate, string toDate,string isActive)
        {
            List<TravelNotificationModel> lstNotificationList = new List<TravelNotificationModel>();
            CustomUserModel user = CurrentUserDetail();
            var notificationlist = objmanageCommon.GetNotificationList(user.ID,Status);
            if (notificationlist.StatusCode == HttpStatusCode.OK)
            {
                IEnumerable<TravelNotificationModel> data = JsonConvert.DeserializeObject<List<TravelNotificationModel>>(notificationlist.Content.ReadAsStringAsync().Result);

                //if (!String.IsNullOrEmpty(lastStatus))
                //{
                //    int TravelLastSatus = Convert.ToInt32(lastStatus);
                //    data = data.Where(x => x.LastStatus == TravelLastSatus).ToList();
                //}
                if (isActive == "true")
                {
                    data = data.Where(x => x.Status == true);
                }
                if (!String.IsNullOrEmpty(fromDate) && !String.IsNullOrEmpty(toDate))
                {
                    data = data.Where(x => x.NotificationDate >= Convert.ToDateTime(fromDate).Date).ToList();
                    ViewBag.fromdate = fromDate;
                    ViewBag.todate = toDate;
                }
                foreach (var item in data)
                {
                    if (item.TravelUser1 != null)
                    {
                        item.Name = item.TravelUser1.FirstName + " " + item.TravelUser1.LastName;
                    }
                }
                lstNotificationList = data.ToList();
            }
            return lstNotificationList;
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (TempData["ChangedPassword"] != null)
            {
                ViewBag.Message1 = TempData["ChangedPassword"].ToString();
            }
            return View(new TravelUserPasswordViewModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(TravelUserPasswordViewModel objTravelUserModel)
        {
            if (!ModelState.IsValid)
            {
                return View(objTravelUserModel);
            }

            //get user by id
            int uid = CurrentUserDetail().ID;

            UserModel objUserModel = objManageUser.GetUserById(uid);
            string encryptedPass = EncryptDecrypt.Encrypt(objTravelUserModel.OldPassword);

            //match for existing password match
            if (objUserModel.Password != encryptedPass)
            {
                ViewBag.Message = String.Format("Incorrect old password");
                return View(objTravelUserModel);
            }
            else if (objTravelUserModel.NewPassword != objTravelUserModel.ConfirmPassword)
            {
                ViewBag.Message = String.Format("New password and confirm password does not match");
                return View(objTravelUserModel);
            }

            objUserModel.Password = EncryptDecrypt.Encrypt(objTravelUserModel.NewPassword);

            objManageUser.UpdateUser(objUserModel);
            ViewBag.Message = "Your Password has been changed successfully";

            TempData["ChangedPassword"] = "Your Password has been changed successfully";
            return View();

        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase postedFile)
        {
            UserModel User;
            int userid = 0;
            if (TempData["userid"] != null && TempData["edituser"] != null)
            {
            
            userid = Convert.ToInt32(TempData["userid"]);
                TempData["userid"] = userid;
                User = objManageUser.GetUserById(userid);
                TempData["uid"] = userid;
            }
            else
            {
                User = objManageUser.GetUserById(CurrentUserDetail().ID);
                TempData["userid"] = null;
                TempData["edituser"] = null;

            }
            
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/ProfileImages/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                System.Guid guid = System.Guid.NewGuid();
                string extension = Path.GetExtension(postedFile.FileName);
                if (Path.GetExtension(postedFile.FileName).ToLower() == ".jpg"
               || Path.GetExtension(postedFile.FileName).ToLower() == ".png"
               || Path.GetExtension(postedFile.FileName).ToLower() == ".jpeg")
                {
                    filePath = path + Path.GetFileName(guid + extension);
                    User.ProfileImage = Path.GetFileName(guid + extension);
                    postedFile.SaveAs(filePath);
                    objManageUser.UpdateUser(User);
                    try
                    {

                        TempData.Add("ProfileImageSaved", "Profile picture has been uploaded successfully");
                    }
                    catch
                    {
                        TempData["ProfileImageSaved"] = "Profile picture has been uploaded successfully";
                    }
                }
                else
                {
                    try
                    {

                        TempData.Add("ProfileImageError", "Only jpeg, jpg and png image types are accepted");
                    }
                    catch
                    {
                        TempData["ProfileImageError"] = "Only jpeg, jpg and png image types are accepted";
                    }
                }
            }
            return RedirectToAction("profile");
        }

       
    }
}