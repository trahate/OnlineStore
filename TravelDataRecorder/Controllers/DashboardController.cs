using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelDataRecorder.Common.TravelDataEnum;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.Controllers
{
    [Authorize]
    public class DashboardController : TravelBaseController
    {
        public DashboardController(IUserService _userRepository, IAdminService _adminRepository, ITravelService _TravelRepository, ITravelStateService _TravelStateRepository, ITravelCityService _TravelCityRepository, INotificationService _NotificationRepository, IProjectManagerService _objProjectManagerRepository, IDbErrorHandlingService _dbErrorHandlingService) : base(_userRepository, _adminRepository, _TravelRepository, _TravelStateRepository, _TravelCityRepository, _NotificationRepository, _objProjectManagerRepository, _dbErrorHandlingService)
        {
        }

        // GET: Dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {

            DashboardViewModel objDashboardVM = new DashboardViewModel();
            objDashboardVM.FromDate = DateTime.UtcNow.AddMonths(-6);
            objDashboardVM.ToDate = DateTime.UtcNow;

            var loggedInUser = CurrentUserDetail();
            objDashboardVM.RoleId = loggedInUser.RoleId;
            objDashboardVM.UserId = loggedInUser.ID;
            var data = GetDashboardData(objDashboardVM);


            return View(data);
        }

        [HttpPost]
        public ActionResult Dashboard(DashboardViewModel objDashboardVM)
        {
            var loggedInUser = CurrentUserDetail();
            objDashboardVM.RoleId = loggedInUser.RoleId;
            objDashboardVM.UserId = loggedInUser.ID;
            var data = GetDashboardData(objDashboardVM);
            return View(data);
        }


        public DashboardViewModel GetDashboardData(DashboardViewModel objDashboardVM)
        {
            if (objDashboardVM.RoleId == (int)UserRoleEnum.ProjectManager)
            {
                var TotalUserData = objManageUser.GetAllUser().Where(x => x.TravelUserRoleMappings.First().RoleID != (int)UserRoleEnum.ProjectManager && x.CreatedOn >= objDashboardVM.FromDate && x.CreatedOn <= objDashboardVM.ToDate);

                objDashboardVM.TotalUser = TotalUserData.Count();
                objDashboardVM.Traveller = TotalUserData.Where(x => x.TravelUserRoleMappings[0].RoleID == (int)UserRoleEnum.User).Count();
                objDashboardVM.ProjectOfficer = TotalUserData.Where(x => x.TravelUserRoleMappings[0].RoleID == (int)UserRoleEnum.Client1).Count();
                objDashboardVM.ChiefEndUser = TotalUserData.Where(x => x.TravelUserRoleMappings[0].RoleID == (int)UserRoleEnum.Client2).Count();

                var alltravelDetailData = objProjectManager.GetAllTravelDetail();
                alltravelDetailData = alltravelDetailData.Where(x => x.Summary_SubmittedDate >= objDashboardVM.FromDate && x.Summary_SubmittedDate <= objDashboardVM.ToDate);
                objDashboardVM.Submitted = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser).Count();
                objDashboardVM.Approved = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
                objDashboardVM.InProgress = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1).Count();
                objDashboardVM.Reject = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2).Count();
                objDashboardVM.ReSubmitted = alltravelDetailData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted).Count();

                objDashboardVM.TotalTravel = TotalUserData.Count();
            }
            else if (objDashboardVM.RoleId == (int)UserRoleEnum.Client1)
            {
                List<TravelDetailModel> travelDataRecord = objProjectManager.GetAllTravelDetail().Where(x => x.Summary_SubmittedDate >= objDashboardVM.FromDate && x.Summary_SubmittedDate <= objDashboardVM.ToDate && x.IsProcedural == (int)ProcedureEnum.Procedural).ToList();

                DateTime Yesterday = DateTime.UtcNow.AddHours(-24);
                objDashboardVM.Pending = travelDataRecord.
                    Where(x => x.Summary_SubmittedDate < Yesterday && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1))
                    .Count();

                //change condition for get current user data
                travelDataRecord = travelDataRecord.Where(x => x.TravelDetailTrails.Select(y => y.ProcessedBy).Contains(objDashboardVM.UserId)).ToList();
                //change condition for get current user data

                objDashboardVM.New = travelDataRecord.Where(x => x.Summary_SubmittedDate >= Yesterday && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1)).Count();
                objDashboardVM.Approved = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
                
                objDashboardVM.Reject = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager).Count();
            }
            else if (objDashboardVM.RoleId == (int)UserRoleEnum.Client2)
            {
                List<TravelDetailModel> travelDataRecord = objProjectManager.GetAllTravelDetail().Where(x => x.Summary_SubmittedDate >= objDashboardVM.FromDate && x.Summary_SubmittedDate <= objDashboardVM.ToDate && (x.IsProcedural == (int)ProcedureEnum.Procedural || x.IsProcedural == (int)ProcedureEnum.DirectFinalStep)).ToList();

                DateTime Yesterday = DateTime.UtcNow.AddHours(-24);
                objDashboardVM.Pending = travelDataRecord.Where(x => x.Summary_SubmittedDate < Yesterday && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1)).Count();

                //change condition for get current user data
                travelDataRecord = travelDataRecord.Where(x => x.TravelDetailTrails.Select(y => y.ProcessedBy).Contains(objDashboardVM.UserId)).ToList();
                //change condition for get current user data

                objDashboardVM.New = travelDataRecord.Where(x => x.Summary_SubmittedDate >= Yesterday && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1) || (x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager && x.IsProcedural == (int)ProcedureEnum.DirectFinalStep)).Count();
               // objDashboardVM.New = travelDataRecord.Where(x => x.Summary_SubmittedDate >= Yesterday && (x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser || x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1)).Count();
                objDashboardVM.Approved = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
                objDashboardVM.Reject = travelDataRecord.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager).Count();
            }
            else if (objDashboardVM.RoleId == (int)UserRoleEnum.User)
            {
                IEnumerable<TravelDetailModel> traveldata = objmanageTravel.GetTravelDetailByUserId(CurrentUserDetail().ID).Where(x => x.Summary_SubmittedDate >= objDashboardVM.FromDate && x.Summary_SubmittedDate <= objDashboardVM.ToDate);
                objDashboardVM.TotalTravel = traveldata.Count();
                objDashboardVM.Approved = traveldata.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2).Count();
                objDashboardVM.InProgress = traveldata.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1).Count();
                objDashboardVM.Reject = traveldata.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager).Count();
                objDashboardVM.Submitted = traveldata.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser).Count();
                objDashboardVM.ReSubmitted = traveldata.Where(x =>x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted).Count();
            }
            return objDashboardVM;

        }

        public ActionResult UserDashboard()
        {
            return PartialView("_UserDashboard");
        }

    }
}