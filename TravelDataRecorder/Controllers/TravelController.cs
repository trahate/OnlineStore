using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using TravelDataRecorder.Common;
using TravelDataRecorder.Common.TravelDataEnum;
using Newtonsoft.Json;
using System.Net;
using System.Data;
using TravelDataRecorder.Common.TravelDataEnum.Helper;

namespace TravelDataRecorder.Controllers
{
    [Authorize]
    public class TravelController : TravelBaseController
    {
        public TravelController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {
        }

        // GET: Travel
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddTravelForm()
        {
            ViewBag.action = "SubmitData";
            if (TempData["TravelResult"] != null)
            {
                ViewBag.Message = TempData["TravelResult"].ToString();
            }
            TravelDetailViewModel traveldetailVm = new TravelDetailViewModel();
            traveldetailVm.UserID = CurrentUserDetail().ID;
            traveldetailVm.traveldetail = new TravelDetailModel();
            traveldetailVm.TravelFromCity = Enumerable.Empty<TravelCityModel>();
            traveldetailVm.TravelToCity = Enumerable.Empty<TravelCityModel>();
            traveldetailVm.TravelReturnCity = Enumerable.Empty<TravelCityModel>();
            UserModel User = objManageUser.GetUserById(CurrentUserDetail().ID);
            if (User.FirstName != null && traveldetailVm.traveldetail.Id == 0)
            {
                traveldetailVm.traveldetail.Summary_TravelerName = User.FirstName + " " + User.LastName;
            }
            IEnumerable<TravelStateModel> States = objmanageCommon.GetAllStates();
            ViewBag.StateList = States;
            return View(traveldetailVm);
        }
        [HttpPost]
        public ActionResult AddTravelForm(TravelDetailViewModel travelDataVm, string start_date, string end_date)
        {
            TravelDetailModel travelData = travelDataVm.traveldetail;
            travelData.UserID = CurrentUserDetail().ID;

            try
            {
                //travelDataVm.traveldetail.Detail_DepartingDate = DateTime.ParseExact(start_date, "MM/dd/yyyy", null);
                //travelDataVm.traveldetail.Detail_ReturningDate = DateTime.ParseExact(end_date, "MM/dd/yyyy", null);
                travelData.Detail_DepartingDate = DateTimeHandling.SetDateTime(travelData.Detail_DepartingDate);
                travelData.Detail_ReturningDate = DateTimeHandling.SetDateTime(travelData.Detail_ReturningDate);
                travelData.Summary_SubmittedDate = DateTimeHandling.SetDateTime(DateTime.Now);
                travelData.LastStatus = Convert.ToInt32(TravelRequestStatusEnum.SubmittedByUser); ;
                if (travelData.Id != 0)
                {
                    travelData.LastStatus = Convert.ToInt32(TravelRequestStatusEnum.Resubmitted);
                    //travelDataVm.id= travelData.Id;
                    objmanageTravel.UpdateTravelDetail(travelDataVm);
                }
                else
                {
                    TravelDetailModel traveldata = objmanageTravel.InsertTraveldata(travelData);
                }
                if (travelData != null)
                {
                    try
                    {
                        TempData.Add("TravelResult", "Travel Information Saved successfully");
                    }
                    catch
                    {
                        TempData["TravelResult"] = "Travel Information Saved successfully";
                    }
                    return RedirectToAction("travellist");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("addtravelform");
        }

        public ActionResult GetCheckDate(string startdate, string enddate)
        {

            TravelDetailModel travelData = new TravelDetailModel();
            //DateTime startdate1 = DateTime.ParseExact(startdate, "MM/dd/yyyy", null);
            //DateTime enddate1 = DateTime.ParseExact(enddate, "MM/dd/yyyy", null);
            DateTime startdate1 = Convert.ToDateTime(startdate);
            DateTime enddate1 = Convert.ToDateTime(enddate);
            travelData.UserID = CurrentUserDetail().ID;
            IEnumerable<TravelDetailModel> traveldetaildata = objmanageTravel.GetTravelDetailByUserId(travelData.UserID);
            //IEnumerable<TravelDetailModel> travel = traveldetaildata.Where(e => e.Detail_DepartingDate.Date >= startdate1 && e.Detail_ReturningDate.Date <= enddate1).ToList();
            if (traveldetaildata.Count() > 0)
            {

                foreach (var item in traveldetaildata)
                {
                    DateTime dbStartTime = item.Detail_DepartingDate;
                    DateTime dbendTime = item.Detail_ReturningDate;
                    DateTime startTime = startdate1;
                    DateTime endTime = enddate1;

                    if (startTime <= dbStartTime && endTime >= dbendTime)
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                    else if ((startTime > dbStartTime) && (startTime < dbendTime))
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                    else if ((endTime > dbStartTime) && (endTime < dbendTime))
                    {
                        return Json("false", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetCity(int stateId)
        {
            var cities = objmanageCommon.GetallCity(stateId);
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        [ChildActionOnly]
        public ActionResult TimeLine(int TravelID)
        {
            TravelDetailViewModel objDetailVM = new TravelDetailViewModel();
            objDetailVM.traveldetail = new TravelDetailModel();

            TravelDetailModel TravelData = objmanageTravel.GetTravelDetail(TravelID);
            return PartialView("_TimeLine", TravelData);
        }

        public ActionResult ShowProgress()
        {
            int travelId = 0;
            if (TempData["travelid"] != null)
            {
                travelId = Convert.ToInt32(TempData["travelid"]);
                TempData["travelid"] = travelId;
            }
            ViewBag.travelId = travelId;
            return View();
        }

        [HttpGet]
        public ActionResult TravelList()
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.LastUrl = Request.UrlReferrer.ToString();
            }
            if (TempData["TravelResult"] != null)
            {
                ViewBag.Message = TempData["TravelResult"].ToString();
            }

            if (TempData["TravelRequest"] != null)
            {
                ViewBag.Message1 = TempData["TravelRequest"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult TravelList(TravelDetailViewModel objTravelDetailVM)
        {
            TravelDetailModel TravelData = objmanageTravel.GetTravelDetail(objTravelDetailVM.id);
            if (objTravelDetailVM.laststepid == TravelData.LastStatus)
            {
                var user = CurrentUserDetail();
                objTravelDetailVM.UserID = user.ID;
                objTravelDetailVM.RoleId = user.RoleId;
                objmanageTravel.UpdateTravelDetailForProcCheck(objTravelDetailVM);

                string msgRejectedBy = "";


                if (objTravelDetailVM.RoleId == (int)UserRoleEnum.ProjectManager && objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager)
                {
                    SendEmailonAction(objTravelDetailVM.id, "Travel Request Rejected", "Your travel request has been rejected by " + EnumHelper<TravelRequestStatusEnum>.GetDisplayValue((TravelRequestStatusEnum.RejectedByProjectManager)).ToString());
                }
                else if (objTravelDetailVM.RoleId == (int)UserRoleEnum.Client1 && objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1)
                {
                    msgRejectedBy = EnumHelper<TravelRequestStatusEnum>.GetDisplayValue((TravelRequestStatusEnum.RejectedByClient1)).ToString();
                    SendEmailonAction(objTravelDetailVM.id, "Travel Request Rejected", "Your travel request has been " + EnumHelper<TravelRequestStatusEnum>.GetDisplayValue((TravelRequestStatusEnum.RejectedByClient1)).ToString(), (int)UserRoleEnum.ProjectManager, msgRejectedBy);
                }
                else if (objTravelDetailVM.RoleId == (int)UserRoleEnum.Client2 && objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2)
                {
                    msgRejectedBy = EnumHelper<TravelRequestStatusEnum>.GetDisplayValue((TravelRequestStatusEnum.RejectedByClient2)).ToString();
                    SendEmailonAction(objTravelDetailVM.id, "Travel Request Rejected", "Your travel request has been " + msgRejectedBy, (int)UserRoleEnum.ProjectManager, msgRejectedBy);
                }
                else if (objTravelDetailVM.RoleId == (int)UserRoleEnum.Client2 && objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2)
                {
                    SendEmailonAction(objTravelDetailVM.id, "Travel Request Approved", "Your travel request has been " + EnumHelper<TravelRequestStatusEnum>.GetDisplayValue((TravelRequestStatusEnum.ApprovedByClient2)).ToString());
                }
            }
            else
            {
                try
                {
                    TempData.Add("TravelRequest", "Travel request is already proceeding");
                }
                catch
                {
                    TempData["TravelRequest"] = "Travel request is already proceeding";
                }
            }
            return RedirectToAction("travellist");
        }

        public void SendEmailonAction(int travelId, string subject, string message, int roleId = 0, string msgRejectedBy = "")
        {
            var userByTravel = objmanageTravel.GetTravelDetail(travelId);
            Dictionary<int, string> userEmail = new Dictionary<int, string>();

            if (roleId == (int)UserRoleEnum.ProjectManager)
            {
                string messageForPM = "Travel Request of user " + userByTravel.TravelUser.Email + "have been ";
                var lstUser = objManageUser.GetUserListByRole(roleId);

                for (int i = 0; i < lstUser.Count; i++)
                {
                    userEmail.Add(i, lstUser[i].Email);
                }

                EmailHelper.SendMail(userEmail, null, null, messageForPM + " " + msgRejectedBy, subject);
            }

            userEmail = new Dictionary<int, string>();

            if (userByTravel != null)
            {
                userEmail.Add(0, userByTravel.TravelUser.Email);
                EmailHelper.SendMail(userEmail, null, null, message, subject);
            }
        }

        public void SetTravelList()
        {
            List<TravelDetailModel> lstRoles = new List<TravelDetailModel>();
            //var result = objmanageTravel.GetAllRole();
            //if (result.StatusCode == HttpStatusCode.OK)
            //{
            //    lstRoles = JsonConvert.DeserializeObject<List<TravelDetailModel>>(result.Content.ReadAsStringAsync().Result);
            //}
        }
        [HttpPost]
        public ActionResult GetTravelUsersByAjax(int draw, int start, int length, string userType, string criteria, string customSearch, int? getFilteredPieChart, DateTime? fromDate, DateTime? toDate)
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
                var lstUsers = new List<TravelDetailModel>();
                if (fromDate != null && toDate != null)
                {
                    lstUsers = GetTravelUserData(userType, criteria, fromDate.Value, toDate.Value).ToList();
                }
                else
                {
                    lstUsers = GetTravelUserData(userType, criteria, searchFilter.FromDate, searchFilter.ToDate).ToList();
                }


                //added for pie chart filter
                var loggedInUser = CurrentUserDetail();

                //set custom order
                List<int> preferences = new List<int>
                                            {
                                                (int)TravelRequestStatusEnum.SubmittedByUser,
                                                (int)TravelRequestStatusEnum.Resubmitted,
                                                (int)TravelRequestStatusEnum.ApprovedByProjectManager,
                                                (int)TravelRequestStatusEnum.ApprovedByClient1,
                                                (int)TravelRequestStatusEnum.RejectedByProjectManager,
                                                (int)TravelRequestStatusEnum.RejectedByClient1,
                                                (int)TravelRequestStatusEnum.RejectedByClient2,
                                                (int)TravelRequestStatusEnum.ApprovedByClient2,
                                            };
                //for PM,Client1,Client2

                if (loggedInUser.RoleId == (int)UserRoleEnum.User)
                {
                    preferences = new List<int>
                                            {
                                                (int)TravelRequestStatusEnum.ApprovedByClient2,
                                                (int)TravelRequestStatusEnum.RejectedByProjectManager,
                                                (int)TravelRequestStatusEnum.RejectedByClient1,
                                                (int)TravelRequestStatusEnum.RejectedByClient2,
                                                (int)TravelRequestStatusEnum.ApprovedByProjectManager,
                                                (int)TravelRequestStatusEnum.ApprovedByClient1,
                                                (int)TravelRequestStatusEnum.Resubmitted,
                                                (int)TravelRequestStatusEnum.SubmittedByUser,
                                            };
                }
                lstUsers = lstUsers.OrderBy(x => preferences.IndexOf(x.LastStatus)).ToList();
                //set custom order


                if (loggedInUser != null)
                {

                    if (loggedInUser.RoleId == (int)UserRoleEnum.Client1 || loggedInUser.RoleId == (int)UserRoleEnum.Client2)
                    {
                        if (criteria != "all" && criteria != "Pending")
                        {
                            lstUsers = lstUsers.Where(x => x.TravelDetailTrails.Select(y => y.ProcessedBy).Contains(loggedInUser.ID)).ToList();
                        }
                        else if (criteria == "all")
                        {
                            var lstWithoutPending = lstUsers.Where(x => !(x.Summary_SubmittedDate < DateTime.UtcNow.AddHours(-24) && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1)) && x.TravelDetailTrails
                               .Select(y => y.ProcessedBy).Contains(loggedInUser.ID))
                            .ToList();

                            lstUsers = lstUsers.Where(x => x.Summary_SubmittedDate < DateTime.UtcNow.AddHours(-24) && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1))
                            .ToList();

                            lstUsers.AddRange(lstWithoutPending);
                        }
                    }
                }

                if (getFilteredPieChart != null)
                {
                    if (getFilteredPieChart == (int)PieChartEnum.Submitted)
                    {
                        lstUsers = lstUsers.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser).ToList();
                    }
                    if (getFilteredPieChart == (int)PieChartEnum.Rejected)
                    {
                        lstUsers = lstUsers.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2).ToList();
                    }
                    if (getFilteredPieChart == (int)PieChartEnum.InProgress)
                    {
                        lstUsers = lstUsers.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1).ToList();
                    }
                    if (getFilteredPieChart == (int)PieChartEnum.Approved)
                    {
                        lstUsers = lstUsers.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).ToList();
                    }
                    if (getFilteredPieChart == (int)PieChartEnum.ReSubmitted)
                    {
                        lstUsers = lstUsers.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted).ToList();
                    }
                }
                //added for pie chart filter

                // 1. Searching  
                if (!string.IsNullOrEmpty(search))
                {
                    lstUsers = lstUsers.Where(c => (c.Summary_ProjectName != null && c.Summary_ProjectName.ToLower().Contains(search.ToLower()))
                              ||
                              (c.Summary_ContractNumber != null && c.Summary_ContractNumber.ToLower().Contains(search.ToLower()))
                              ||
                              (c.Summary_SubmittedDate.ToString() != null && c.Summary_SubmittedDate.ToString("MM/dd/yyyy").ToLower().Contains(search.ToLower()))
                              ||
                              (c.Detail_TravelSitePOC != null && c.Detail_TravelSitePOC.ToString().ToLower().Contains(search.ToLower()))
                              ||
                              (c.Cost_TotalExpense.ToString() != null && c.Cost_TotalExpense.ToString().ToLower().Contains(search.ToLower()))
                              ||
                              (c.Detail_AirportDepartingFrom != null && c.Detail_AirportDepartingFrom.ToString().ToLower().Contains(search.ToLower()))
                              ||
                              (c.Detail_DepartingDate.ToString() != null && c.Detail_DepartingDate.ToString("MM/dd/yyyy").ToLower().Contains(search.ToLower()))
                              ||
                              (c.Detail_ReturningDate.ToString() != null && c.Detail_ReturningDate.ToString("MM/dd/yyyy").ToLower().Contains(search.ToLower()))
                             ||
                             (c.Summary_TravelerName.ToString() != null && c.Summary_TravelerName.ToString().ToLower().Contains(search.ToLower()))).ToList();
                }


                // 2. Get the total record count  

                // 3. Sorting  



                var filteredData = lstUsers as IEnumerable<TravelDetailModel>;
                if (OrderColumnName != "LastStatus")
                {
                    Func<TravelDetailModel, string> orderingFunction = (c => OrderColumnName == "Summary_ProjectName" ? c.Summary_ProjectName :
                                              OrderColumnName == "Summary_ContractNumber" ? c.Summary_ContractNumber.ToString() :
                                              OrderColumnName == "Summary_SubmittedDate" ? c.Summary_SubmittedDate.ToString() :
                                              OrderColumnName == "Detail_TravelSitePOC" ? c.Detail_TravelSitePOC :
                                              OrderColumnName == "Cost_TotalExpense" ? c.Cost_TotalExpense.ToString() :
                                              OrderColumnName == "Detail_AirportDepartingFrom" ? c.Detail_AirportDepartingFrom.ToString() :
                                              OrderColumnName == "Detail_DepartingDate" ? c.Detail_DepartingDate.ToString() :
                                              OrderColumnName == "Detail_ReturningDate" ? c.Detail_ReturningDate.ToString() :
                                                OrderColumnName == "Summary_TravelerName" ? c.Summary_TravelerName.ToString() : c.Summary_ProjectName);
                    if (sortColumnDir == "asc")
                        filteredData = filteredData.OrderBy(orderingFunction);
                    else
                        filteredData = filteredData.OrderByDescending(orderingFunction);
                }

                switch (criteria)
                {
                    case "Approved":

                        filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2);
                        break;
                    case "InProgress":
                        filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1);
                        break;
                    case "Pending":
                        filteredData = filteredData.Where(x => x.Summary_SubmittedDate < DateTime.UtcNow.AddHours(-24) && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1));
                        break;
                    case "Reject":
                        filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager);
                        break;
                    case "Submitted":
                        filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser);
                        break;
                    case "ReSubmitted":
                        filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted);
                        break;
                    case "New":
                        if (CurrentUserDetail().RoleId == (int)UserRoleEnum.Client1)
                        {

                            filteredData = filteredData.Where(x => x.Summary_SubmittedDate >= DateTime.UtcNow.AddHours(-24) && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1));
                        }
                        else if (CurrentUserDetail().RoleId == (int)UserRoleEnum.Client2)
                        {
                            DateTime Yesterday = DateTime.UtcNow.AddHours(-24);
                            filteredData = filteredData.Where(x => x.Summary_SubmittedDate >= Yesterday && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1) || (x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager && x.IsProcedural == (int)ProcedureEnum.DirectFinalStep));
                        }
                        break;
                    case "TotalTravel":
                        filteredData = filteredData;
                        break;
                }
                // 4. Filtering  
                recordsTotal = filteredData.Count();
                filteredData = filteredData.Skip(skip).Take(pageSize).ToList();

                // 5. Get the filtered record count  
                recordFilteredTotal = filteredData.Count();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordFilteredTotal, data = filteredData }, JsonRequestBehavior.AllowGet);
            }


            return View();
        }
        private List<TravelDetailModel> GetTravelUserData(string lastStatus, string criteria, DateTime? fromDate, DateTime? toDate)
        {
            List<TravelDetailModel> lstTravelList = new List<TravelDetailModel>();
            var user = CurrentUserDetail();
            var TravelData = objManageUser.GetTravelDataList(user.RoleId, user.ID);

            if (TravelData.StatusCode == HttpStatusCode.OK)
            {
                IEnumerable<TravelDetailModel> data = JsonConvert.DeserializeObject<List<TravelDetailModel>>(TravelData.Content.ReadAsStringAsync().Result);
                data.ToList().ForEach(x => x.Cost_TotalExpense = Math.Round(x.Cost_TotalExpense.Value, 2));
                if (!String.IsNullOrEmpty(lastStatus))
                {
                    int TravelLastSatus = Convert.ToInt32(lastStatus);
                    data = data.Where(x => x.LastStatus == TravelLastSatus).ToList();
                }
                if (fromDate != null && toDate != null)
                {
                    data = data.Where(x => x.Summary_SubmittedDate >= fromDate && x.Summary_SubmittedDate <= toDate).ToList();
                    ViewBag.fromdate = fromDate;
                    ViewBag.todate = toDate;
                }
                foreach (var item in data)
                {
                    item.SubmittedDate = item.Summary_SubmittedDate.ToString("MM/dd/yyyy");
                    item.DepartingDate = item.Detail_DepartingDate.ToString("MM/dd/yyyy");
                    item.ReturningDate = item.Detail_ReturningDate.ToString("MM/dd/yyyy");
                }
                lstTravelList = data.ToList();
            }
            return lstTravelList;
        }


        [HttpGet]
        public ActionResult TravelAllList()
        {
            IEnumerable<TravelDetailModel> lstTravelList = objProjectManager.GetAllTravelDetail();
            lstTravelList.ToList().ForEach(x => x.Cost_TotalExpense = Math.Round(x.Cost_TotalExpense.Value, 2));
            return View("TravelList", lstTravelList);
        }
        [HttpGet]
        public ActionResult viewdata(int travelid, string ViewOnly, string view)
        {
            TempData["view"] = view;
            TempData["travelid"] = travelid;
            TempData["ViewOnly"] = ViewOnly;

            if (ViewOnly == "showprogress")
            {
                return RedirectToAction("showprogress");
            }

            else
            {
                return RedirectToAction("showtraveldata");
            }
        }
        [HttpGet]
        public ActionResult showtraveldata()
        {
            string view = "";
            int travelid = 0;
            string ViewOnly = "";
            if (TempData["view"] != null)
            {
                view = TempData["view"].ToString();
                TempData["view"] = view;
            }
            if (TempData["travelid"] != null)
            {
                travelid = Convert.ToInt32(TempData["travelid"]);
                TempData["travelid"] = travelid;
            }
            if (TempData["ViewOnly"] != null)
            {
                ViewOnly = TempData["ViewOnly"].ToString();
                TempData["ViewOnly"] = ViewOnly;
            }
            if (view == "view")
            {
                ViewBag.ShowNotify = "view";
            }
            ViewBag.action = "ViewData";
            TravelDetailViewModel traveldetailVm = new TravelDetailViewModel();
            traveldetailVm.traveldetail = objmanageTravel.GetTravelDetail(travelid);
            int roleid = traveldetailVm.traveldetail.TravelUser.TravelUserRoleMappings.Where(x => x.UserID == traveldetailVm.traveldetail.UserID).Select(x => x.RoleID).FirstOrDefault();

            if (roleid == (int)UserRoleEnum.User && (traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByClient1) || traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByClient2) || traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByProjectManager)))
            {
                ViewBag.action = "ResubmitTraveldata";
            }

            if (ViewOnly != "viewonly" && (traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByClient1) || traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByClient2) || traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByProjectManager)))
            {
                ViewBag.action = "ResubmitTraveldata";
            }
            if (traveldetailVm.traveldetail != null)
            {
                traveldetailVm.traveldetail.Cost_CostOfAirFare = Math.Round(traveldetailVm.traveldetail.Cost_CostOfAirFare.Value, 2);
                traveldetailVm.traveldetail.Cost_Hotel = Math.Round(traveldetailVm.traveldetail.Cost_Hotel.Value, 2);
                traveldetailVm.traveldetail.Cost_Miscellaneous = Math.Round(traveldetailVm.traveldetail.Cost_Miscellaneous.Value, 2);
                traveldetailVm.traveldetail.Cost_PerDiemRate = Math.Round(traveldetailVm.traveldetail.Cost_PerDiemRate.Value, 2);
                traveldetailVm.traveldetail.Cost_RegistrationFee = Math.Round(traveldetailVm.traveldetail.Cost_RegistrationFee.Value, 2);
                traveldetailVm.traveldetail.Cost_RentalCar = Math.Round(traveldetailVm.traveldetail.Cost_RentalCar.Value, 2);
                traveldetailVm.traveldetail.Cost_TotalPerDiem = Math.Round(traveldetailVm.traveldetail.Cost_TotalPerDiem.Value, 2);
                traveldetailVm.traveldetail.Cost_TotalExpense = Math.Round(traveldetailVm.traveldetail.Cost_TotalExpense.Value, 2);
                traveldetailVm.travelstate = objmanageCommon.GetAllStates();
                if (traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByClient1) || traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByClient2) || traveldetailVm.traveldetail.LastStatus == Convert.ToInt32(TravelRequestStatusEnum.RejectedByProjectManager))
                {
                    ViewBag.StateList = traveldetailVm.travelstate;
                }
                traveldetailVm.travelcity = objmanageCommon.GetCityList();
                traveldetailVm.TravelFromCity = objmanageCommon.GetallCity(traveldetailVm.traveldetail.Detail_TravelingFromState);
                traveldetailVm.TravelToCity = objmanageCommon.GetallCity(traveldetailVm.traveldetail.Detail_TravelingToState);
                traveldetailVm.TravelReturnCity = objmanageCommon.GetallCity(traveldetailVm.traveldetail.Detail_ReturningToState);
                IEnumerable<UserModel> allUser = objManageUser.GetAllUser();
                TravelDetailViewModel objTravelVM = new TravelDetailViewModel();
                traveldetailVm.lstTraveldetailVM = new List<TravelDetailViewModel>();

                foreach (var item in traveldetailVm.traveldetail.TravelDetailTrails.Where(x => x.StatusId != Convert.ToInt32(TravelRequestStatusEnum.SubmittedByUser)))
                {
                    UserModel user = allUser.FirstOrDefault(x => x.ID == item.ProcessedBy);

                    objTravelVM = new TravelDetailViewModel();
                    objTravelVM.UserName = user == null ? "" : user.FirstName + " " + user == null ? "" : user.LastName;
                    objTravelVM.Date = item.Date;
                    objTravelVM.Comments = item.Comments;
                    objTravelVM.RoleName = user == null ? "" : user.TravelUserRoleMappings[0].TravelRole.DisplayName;
                    objTravelVM.LastStatus = Convert.ToInt16(item.StatusId);
                    traveldetailVm.lstTraveldetailVM.Add(objTravelVM);

                }

            }
            return View("addtravelform", traveldetailVm);
        }

    }
}