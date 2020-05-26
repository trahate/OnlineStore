using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TravelDataRecorder.Common;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using PagedList;
using TravelDataRecorder.Common.TravelDataEnum;
using System.Net;
using Newtonsoft.Json;

namespace TravelDataRecorder.Controllers
{
    [Authorize]
    public class AdminController : TravelBaseController
    {
        public AdminController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {
        }

        public ActionResult AddUser()
        {
            TravelUserViewModel data = new TravelUserViewModel();
            IEnumerable<TravelRoleModel> roles = objmanageAdmin.GetAllRole();
            ViewBag.RoleList = roles.Where(r => r.Id != (int)UserRoleEnum.ProjectManager);
            return View(data);
        }
        [HttpPost]
        public ActionResult AddUser(TravelUserViewModel traveluser)
        {
            try
            {
                var userData = objmanageAdmin.GetAllUser();
                List<UserModel> UserListData = new List<UserModel>();
                if (userData.StatusCode == HttpStatusCode.OK)
                {
                    UserListData = JsonConvert.DeserializeObject<List<UserModel>>(userData.Content.ReadAsStringAsync().Result).ToList();
                }
                bool isValid = UserListData.Exists(p => p.Email.Equals(traveluser.Email, StringComparison.CurrentCultureIgnoreCase));
                if (isValid == true)
                {

                    ViewBag.Message = String.Format("Email already exist");
                    IEnumerable<TravelRoleModel> roles = objmanageAdmin.GetAllRole();
                    ViewBag.RoleList = roles.Where(r => r.Id != (int)UserRoleEnum.ProjectManager);
                    return View(traveluser);
                }
                else
                {
                    UserModel updatedData = objmanageAdmin.AddUser(traveluser);
                    bool Sent = SendOTP(updatedData);
                    try
                    {
                        TempData.Add("AddUser", "User has been added successfully");
                    }
                    catch
                    {
                        TempData["AddUser"] = "User has been added successfully";
                    }
                    return RedirectToAction("userlist");// Redirect to userlist 
                }
            }
            catch (Exception ex)
            {

            }
            return View(traveluser);
        }

        public bool SendOTP(UserModel updatedData)
        {
            string BaseUrl = "";
            if (Request.Url.Authority.Contains("localhost"))
            {
                BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/traveldatarecorder/home/GeneratePassword/?key=" + updatedData.GeneratePasswordKey;
            }
            else
            {
                BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/home/GeneratePassword/?key=" + updatedData.GeneratePasswordKey;
            }

            //to address
            Dictionary<int, string> toAddress = new Dictionary<int, string>();
            toAddress.Add(0, updatedData.Email);
            //to address

            //send email
            //EmailHelper objEmailHelper = new EmailHelper(_dbErrorHandlingService);
            ////objEmailHelper.SendMail(toAddress, null, null, "Hi your OTP is " + OTP, "Forgot Password OTP");
            bool SuccessSent = EmailHelper.SendMail(toAddress, null, null, "Hi your Activation link is " + BaseUrl, "Generate Password Link");
            //send email
            return SuccessSent;

        }
        public ActionResult UserList()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.LastUrl = Request.UrlReferrer.ToString();
            }

            if (TempData["DeleteResult"] != null)
            {
                ViewBag.Message = TempData["DeleteResult"].ToString();
            }
            if (TempData["AddUser"] != null)
            {
                ViewBag.Message = TempData["AddUser"].ToString();
                ViewBag.IsAdded = true;
            }

            SetUserList();
            ViewBag.Users = "Manage User";

            return View();
        }
        public void SetUserList()
        {
            List<TravelRoleModel> lstRoles = new List<TravelRoleModel>();
            var result = objManageUser.GetAllRole();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                lstRoles = JsonConvert.DeserializeObject<List<TravelRoleModel>>(result.Content.ReadAsStringAsync().Result);
            }
            ViewBag.userslist = PagesizeHelper.GetAllDesignatedUsers(lstRoles.Where(x => x.Id != (int)UserRoleEnum.ProjectManager).ToList());
        }
        [HttpPost]
        public ActionResult GetUsersByAjax(int draw, int start, int length, string userType, string criteria, string customSearch, DateTime? fromDate, DateTime? toDate)
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
                var lstUsers = new List<UserModel>();
                if (fromDate != null && toDate != null)
                {
                    lstUsers = GetUserData(userType, criteria, fromDate, toDate);
                }
                else
                {
                    lstUsers = GetUserData(userType, criteria, searchFilter.FromDate, searchFilter.ToDate);
                }


                // 1. Searching  
                if (!string.IsNullOrEmpty(search))
                {
                    lstUsers = lstUsers.Where(c => (c.FirstName != null && c.FirstName.ToLower().Contains(search.ToLower()))
                              ||
                              (c.LastName != null && c.LastName.ToLower().Contains(search.ToLower()))
                              ||
                              (c.Email != null && c.Email.ToLower().Contains(search.ToLower()))
                              ||
                              (c.ContactNumber != null && c.ContactNumber.ToString().ToLower().Contains(search.ToLower()))
                              ||
                              (c.RoleName != null && c.RoleName.ToLower().Contains(search.ToLower()))
                              ||
                              (c.Address != null && c.Address.ToString().ToLower().Contains(search.ToLower()))).ToList();
                }

                // 2. Get the total record count  

                // 3. Sorting  



                var filteredData = lstUsers as IEnumerable<UserModel>;
                Func<UserModel, string> orderingFunction = (c => OrderColumnName == "FirstName" ? c.FirstName :
                                          OrderColumnName == "LastName" ? c.LastName :
                                          OrderColumnName == "Email" ? c.Email :
                                          OrderColumnName == "ContactNumber" ? c.ContactNumber.ToString() :
                                          OrderColumnName == "RoleName" ? c.RoleName :
                                          OrderColumnName == "Address" ? c.Address : c.FirstName);

                if (sortColumnDir == "asc")
                    filteredData = filteredData.OrderBy(orderingFunction);
                else
                    filteredData = filteredData.OrderByDescending(orderingFunction);

                //if (criteria == "User")
                //{
                //    filteredData = filteredData.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.User);
                //}
                switch (criteria)
                {
                    case "User":
                        filteredData = filteredData.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.User);

                        break;
                    case "Client1":
                        filteredData = filteredData.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.Client1);

                        break;
                    case "Client2":
                        filteredData = filteredData.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.Client2);

                        break;
                }
                // 4. Filtering  
                recordsTotal = filteredData.Count();
                filteredData = filteredData.Skip(skip).Take(pageSize).ToList();
                recordFilteredTotal = filteredData.Count();
                // 5. Get the filtered record count  


                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordFilteredTotal, data = filteredData }, JsonRequestBehavior.AllowGet);
            }


            return View();
        }
        private List<UserModel> GetUserData(string userType, string criteria, DateTime? fromDate, DateTime? toDate)
        {
            List<UserModel> lstUserobject = new List<UserModel>();
            var userData = objmanageAdmin.GetAllUser();
            if (userData.StatusCode == HttpStatusCode.OK)
            {
                IEnumerable<UserModel> data = JsonConvert.DeserializeObject<List<UserModel>>(userData.Content.ReadAsStringAsync().Result);
                if (!String.IsNullOrEmpty(userType))
                {
                    int UserRoleId = Convert.ToInt32(userType);
                    data = data.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == UserRoleId).ToList();
                }
                if (fromDate != null && toDate != null)
                {
                    data = data.Where(x => x.CreatedOn >= Convert.ToDateTime(fromDate) && x.CreatedOn <= Convert.ToDateTime(toDate)).ToList();
                    ViewBag.fromdate = fromDate;
                    ViewBag.todate = toDate;
                }

                foreach (var item in data)
                {
                    item.RoleName = item.TravelUserRoleMappings.FirstOrDefault().TravelRole.Name;
                    lstUserobject.Add(item);
                }
                //to remove programManager from list
                lstUserobject = lstUserobject.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID != (int)UserRoleEnum.ProjectManager).ToList();
            }
            return lstUserobject;
        }




        public ActionResult DeleteUser(int id)
        {
            try
            {
                UserModel UserData = objManageUser.GetUserById(id);
                objmanageAdmin.DeleteUser(UserData);
                try
                {

                    TempData.Add("DeleteResult", "Deleted");
                }
                catch
                {
                    TempData["DeleteResult"] = "Deleted";
                }




                return RedirectToAction("userlist");
            }
            catch (Exception ex)
            {
                TempData.Add("DeleteResult", ex.Message);
                return RedirectToAction("userlist");
            }
        }
    }

}
