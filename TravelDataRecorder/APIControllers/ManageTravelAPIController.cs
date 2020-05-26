using System;
using System.Collections.Generic;
using System.Web.Http;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Common.TravelDataEnum;

namespace TravelDataRecorder.APIControllers
{
    public class ManageTravelAPIController : ApiController
    {
        ITravelService _travelRepository;
        public ManageTravelAPIController(ITravelService travelRepository)
        {
            _travelRepository = travelRepository;
        }
        [HttpPost]
        public TravelDetailModel InsertTraveldata(TravelDetailModel TravelData)
        {
            return _travelRepository.AddTravelDetail(TravelData);
        }
        [HttpPost]
        public TravelDetailModel GetTravelDetail(int travelid)
        {
            TravelDetailModel traveldata = _travelRepository.GetTravelDetail(travelid);
            return traveldata;
        }
        [HttpPost]
        public IEnumerable<TravelDetailModel> GetTravelDetailByUserId(int userid)
        {
            IEnumerable<TravelDetailModel> traveldata = _travelRepository.GetTravelDetailByUserId(userid);
            return traveldata;
        }
        [HttpPut]
        public void UpdateTravelDetailForProcCheck(TravelDetailViewModel objTravelDetailVM)
        {
            var travelDetail = new TravelDetailModel();
            var travelDetailTrail = new TravelDetailTrailModel();

            travelDetail = _travelRepository.GetTravelDetail(objTravelDetailVM.id);

            if (objTravelDetailVM.RoleId == (int)UserRoleEnum.ProjectManager)
            {
                if (objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager)
                {
                    travelDetail.IsProcedural = (int)ProcedureEnum.Default;
                }
                else if (objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager)
                {
                    if (objTravelDetailVM.IsProcedural == (int)ProcedureEnum.Procedural)
                    {
                        travelDetailTrail.StepNotes = (int)StepNotesEnum.TravelDetailAcceptedByPMForProc;
                        travelDetailTrail.NextActionRole = (int)UserRoleEnum.Client1;
                    }
                    else if (objTravelDetailVM.IsProcedural == (int)ProcedureEnum.DirectFinalStep)
                    {
                        travelDetailTrail.StepNotes = (int)StepNotesEnum.TravelDetailAcceptedByPMForProc;
                        travelDetailTrail.NextActionRole = (int)UserRoleEnum.Client2;
                    }
                }
                travelDetail.IsProcedural = objTravelDetailVM.IsProcedural;
                travelDetail.LastStatus = objTravelDetailVM.LastStatus;
            }
            else if (objTravelDetailVM.RoleId == (int)UserRoleEnum.Client1)
            {
                if (objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1)
                {
                    travelDetailTrail.NextActionRole = null;
                    travelDetailTrail.StepNotes = (int)StepNotesEnum.RejectedByClient1;
                }
                else if (objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1)
                {
                    travelDetailTrail.NextActionRole = (int)UserRoleEnum.Client2;
                    travelDetailTrail.StepNotes = (int)StepNotesEnum.ApprovedByClient1;
                }
                travelDetail.LastStatus = objTravelDetailVM.LastStatus;
            }
            else if (objTravelDetailVM.RoleId == (int)UserRoleEnum.Client2)
            {
                if (objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2)
                {
                    travelDetailTrail.NextActionRole = null;
                    travelDetailTrail.StepNotes = (int)StepNotesEnum.RejectedByClient2;
                }
                else if (objTravelDetailVM.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2)
                {
                    travelDetailTrail.StepNotes = (int)StepNotesEnum.ApprovedByClient2;
                    travelDetailTrail.NextActionRole = null;
                }
                travelDetail.LastStatus = objTravelDetailVM.LastStatus;
            }
                        _travelRepository.UpdateTravelDetail(travelDetail);


            //travel detail trail logic start
            travelDetailTrail.StatusId = objTravelDetailVM.LastStatus;
            travelDetailTrail.TravelId = objTravelDetailVM.id;
            travelDetailTrail.ProcessedBy = objTravelDetailVM.UserID;
            travelDetailTrail.Comments = objTravelDetailVM.Comments;
            travelDetailTrail.Date = DateTime.Now;

            _travelRepository.AddTravelDetailTrail(travelDetailTrail);

            //travel detail trail logic end

       
        }

        public IEnumerable<TravelDetailModel> GetFilteredData(FilterViewModel filterViewModel)
        {
            return _travelRepository.GetFilteredData(filterViewModel);
        }

        public void UpdateTravelDetail(TravelDetailViewModel objTravelDetailVM)
        {
            _travelRepository.UpdateTravelDetail(objTravelDetailVM.traveldetail);

            TravelDetailTrailModel travelDetailTrail = new TravelDetailTrailModel();
            travelDetailTrail.StatusId = objTravelDetailVM.traveldetail.LastStatus;
            travelDetailTrail.TravelId = objTravelDetailVM.traveldetail.Id;
            travelDetailTrail.ProcessedBy = objTravelDetailVM.traveldetail.UserID;
             travelDetailTrail.Date = DateTime.Now;

            _travelRepository.AddTravelDetailTrail(travelDetailTrail);
        }

    }
}
