using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using TravelDataRecorder.Common.TravelDataEnum;
using System;
using TravelDataRecorder.Common;

namespace TravelDataRecorder.Controllers
{
    [Authorize]
    public class ProjectManagerController : TravelBaseController
    {
        public ProjectManagerController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {
        }


        // GET: ProjectManager
        [HttpGet]
        public ActionResult Index()
        {
            DashboardViewModel objDashboardVM = new DashboardViewModel();
            objDashboardVM.FromDate = DateTime.Now.AddMonths(-6);
            objDashboardVM.ToDate = DateTime.Now;
            if (TempData["FilterModelData"] != null)
            {
                var dashboardModelData = (DashboardViewModel)TempData["FilterModelData"];
                objDashboardVM.FromDate = dashboardModelData.FromDate;
                objDashboardVM.ToDate = dashboardModelData.ToDate;
            }
            var TotalUserData = objManageUser.GetAllUser().Where(x => x.TravelUserRoleMappings.First().RoleID != (int)UserRoleEnum.ProjectManager && x.CreatedOn >= objDashboardVM.FromDate && x.CreatedOn <= objDashboardVM.ToDate);
            DashboardViewModel dashboard = new DashboardViewModel();

            dashboard.TotalUser = TotalUserData.Count();
            dashboard.Traveller = TotalUserData.Where(x => x.TravelUserRoleMappings[0].RoleID == (int)UserRoleEnum.User).Count();
            dashboard.ProjectOfficer = TotalUserData.Where(x => x.TravelUserRoleMappings[0].RoleID == (int)UserRoleEnum.Client1).Count();
            dashboard.ChiefEndUser = TotalUserData.Where(x => x.TravelUserRoleMappings[0].RoleID == (int)UserRoleEnum.Client2).Count();

            var alltravelDetailData = objProjectManager.GetAllTravelDetail();
            alltravelDetailData = alltravelDetailData.Where(x => x.Summary_SubmittedDate >= objDashboardVM.FromDate && x.Summary_SubmittedDate <= objDashboardVM.ToDate);
            dashboard.Submitted = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser).Count();
            dashboard.Approved = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
            dashboard.InProgress = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1).Count();
            dashboard.Reject = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2).Count();

            return PartialView("_ProgramManagerDashboard", dashboard);
        }


        [HttpGet]
        public ActionResult TravelList(string UserTimezoneOffSet)
        {
            List<TravelDetailModel> TravelList = objProjectManager.GetAllTravelDetail().Where(x => x.Detail_DepartingDate.Month == DateTime.UtcNow.Month).ToList();
            List<Calendar_EventsVM> EventList = new List<Calendar_EventsVM>();
            foreach (var item in TravelList)
            {
                DateTime startdate = DateTimeHandling.GetDateTime(item.Detail_DepartingDate);
                DateTime enddate = DateTimeHandling.GetDateTime(item.Detail_ReturningDate);
                Calendar_EventsVM cal = new Calendar_EventsVM();
                cal.id = item.Id;
                cal.start = startdate.ToString();//item.Detail_DepartingDate.ToString("MM/dd/yyyy");
                cal.end = enddate.ToString(); //item.Detail_ReturningDate.ToString("MM/dd/yyyy");
                cal.title = item.Summary_TravelerName + "\n " + startdate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + " - " + enddate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "\n " + item.Summary_ProjectName;
                cal.desc = item.Summary_TravelerName;
                cal.status = item.LastStatus.ToString();
                string colour = "";
                string statusCss = string.Empty;
                switch (item.LastStatus)
                {
                    case (int)TravelRequestStatusEnum.SubmittedByUser:
                        colour = "#63c2de";
                        statusCss = "cal-blue blue";
                        break;
                    case (int)TravelRequestStatusEnum.Resubmitted:
                        colour = "#6b73e7";
                        statusCss = "cal-blue-dark dark-blue";
                        break;
                    case (int)TravelRequestStatusEnum.InProcess:
                    case (int)TravelRequestStatusEnum.ApprovedByProjectManager:
                    case (int)TravelRequestStatusEnum.ApprovedByClient1:
                        colour = "#f8cb00";
                        statusCss = "cal-yellow yellow";
                        break;
                    case (int)TravelRequestStatusEnum.ApprovedByClient2:
                        colour = "#45dcbd";
                        statusCss = "cal-green green";
                        break;


                    case (int)TravelRequestStatusEnum.RejectedByClient1:
                    case (int)TravelRequestStatusEnum.RejectedByClient2:
                    case (int)TravelRequestStatusEnum.RejectedByProjectManager:
                        colour = "#f96d6c";
                        statusCss = "cal-red red-cl";
                        break;
                }
                cal.className = statusCss;
                cal.color = colour;
                EventList.Add(cal);
            }

            //foreach (var item in EventList)
            //{
            //    DateTime dt = Convert.ToDateTime(item.start);
            //    DateTime dtEnd = Convert.ToDateTime(item.end);
            //    item.start = dt.ToString("yyyy-MM-dd") + "T10:30:00Z";
            //    item.end = dtEnd.ToString("yyyy-MM-dd") + "T10:30:00Z";
            //}
         
            EventList.ForEach(x => { x.start = (Convert.ToDateTime(x.start).ToString("yyyy-MM-dd") + "T10:30:00Z"); x.end = (Convert.ToDateTime(x.end).ToString("yyyy-MM-dd") + "T10:30:00Z"); });
            var TravelListJSONData = Json(EventList, JsonRequestBehavior.AllowGet);

            return TravelListJSONData;
        }


        [HttpGet]
        public ActionResult Notificationlist()
        {
            ViewBag.notification = true;
            ViewBag.Breadcrumb = "Notification";
            return View();
        }
    }
}