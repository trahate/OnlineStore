using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelDataRecorder.Common;
using TravelDataRecorder.Common.TravelDataEnum;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.Controllers
{
    [Authorize]
    public class ClientController : TravelBaseController
    {
        public ClientController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {
        }


      //  GET: Client
      [HttpGet]
        public ActionResult Index()
        {
            List<TravelDetailModel> travelDataRecord = objProjectManager.GetAllTravelDetail().Where(x => x.Summary_SubmittedDate >= DateTime.Now.AddMonths(-6) && x.IsProcedural == (int)ProcedureEnum.Procedural).ToList();
            DashboardViewModel dashboard = new DashboardViewModel();
            
                DateTime Yesterday = DateTime.UtcNow.AddDays(-1);
                dashboard.New = travelDataRecord.Where(x => x.Summary_SubmittedDate>=Yesterday).Count();
                dashboard.Approved = travelDataRecord.Where(x=>x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
                dashboard.InProgress = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1).Count();
            dashboard.Reject = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager).Count();
             
            return PartialView("_ProjectOfficerDashboard", dashboard);


        }
        [HttpGet]
        public ActionResult ChiefEndUser()
        {
            List<TravelDetailModel> travelDataRecord = objProjectManager.GetAllTravelDetail().Where(x => x.Summary_SubmittedDate >= DateTime.Now.AddMonths(-6) && (x.IsProcedural == (int)ProcedureEnum.Procedural || x.IsProcedural == (int)ProcedureEnum.DirectFinalStep)).ToList();
            DashboardViewModel dashboard = new DashboardViewModel();
            try
            {
                DateTime Yesterday = DateTime.UtcNow.AddDays(-1);
                dashboard.New = travelDataRecord.Where(x => x.Summary_SubmittedDate >= Yesterday).Count();
                dashboard.Approved = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
                dashboard.Pending = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser|| x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1).Count();
                dashboard.Reject = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager).Count();
            }
            catch
            {

            }

            return PartialView("_ChiefEndUserDashboard", dashboard);


        }
    }
}