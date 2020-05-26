using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Common.TravelDataEnum;
using TravelDataRecorder.Common.TravelDataEnum.Helper;
using TravelDataRecorder.Database.DAL;

namespace TravelDataRecorder.Database.Repository
{
    public class TravelRepository : ITravelRepository
    {
        private readonly TravelDataRecoderEntities _context;
        public TravelRepository()
        {
            _context = new TravelDataRecoderEntities();
        }

        public TravelDetail AddTravelDetail(TravelDetail travelDetailData)
        {
            _context.TravelDetails.Add(travelDetailData);
            _context.SaveChanges();
            //string Message = EnumHelper<TravelRequestStatusEnum>.GetDisplayValue((TravelRequestStatusEnum)travelDetailData.LastStatus);
            string Message = "Submitted By User";
            int UserId = _context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.ProjectManager).Select(e => e.ID).FirstOrDefault();
            bool Status = true;
            int stepid = (int)travelDetailData.LastStatus;

            InsertNotification(Message, UserId, Status, stepid, travelDetailData.Id, travelDetailData.UserID);
            return travelDetailData;
        }

        public void DeleteTravelDetail(TravelDetail TravelDetailObj)
        {
            _context.Entry(TravelDetailObj).State = System.Data.Entity.EntityState.Deleted;
            _context.SaveChanges();
        }



        public TravelDetail GetTravelDetail(int id)
        {
            return _context.TravelDetails.FirstOrDefault(x => x.Id == id);
        }



        public void UpdateTravelDetail(TravelDetail traveldetail)
        {
            TravelDetail updateTravelDetail = _context.TravelDetails.FirstOrDefault(x => x.Id == traveldetail.Id);
            updateTravelDetail.Summary_ProjectName = traveldetail.Summary_ProjectName;
            updateTravelDetail.Summary_PrimeContractor = traveldetail.Summary_PrimeContractor;
            updateTravelDetail.Summary_ContractNumber = traveldetail.Summary_ContractNumber;
            updateTravelDetail.Summary_TaskOrder = traveldetail.Summary_TaskOrder;
            updateTravelDetail.Summary_CorName = traveldetail.Summary_CorName;
            updateTravelDetail.Summary_SubmittedDate = traveldetail.Summary_SubmittedDate;
            updateTravelDetail.Summary_TravelerName = traveldetail.Summary_TravelerName;
            updateTravelDetail.Detail_AirportDepartingFrom = traveldetail.Detail_AirportDepartingFrom;
            updateTravelDetail.Detail_TravelingFromCity = traveldetail.Detail_TravelingFromCity;
            updateTravelDetail.Detail_TravelingFromState = traveldetail.Detail_TravelingFromState;
            updateTravelDetail.Detail_TravelingToCity = traveldetail.Detail_TravelingToCity;
            updateTravelDetail.Detail_TravelingToState = traveldetail.Detail_TravelingToState;
            updateTravelDetail.Detail_ReturningToCity = traveldetail.Detail_ReturningToCity;
            updateTravelDetail.Detail_ReturningToState = traveldetail.Detail_ReturningToState;
            updateTravelDetail.Detail_DepartingDate = traveldetail.Detail_DepartingDate;
            updateTravelDetail.Detail_ReturningDate = traveldetail.Detail_ReturningDate;
            updateTravelDetail.Detail_TotalDays = traveldetail.Detail_TotalDays;
            updateTravelDetail.Detail_TravelPurpose = traveldetail.Detail_TravelPurpose;
            updateTravelDetail.Detail_TravelSitePOC = traveldetail.Detail_TravelSitePOC;
            updateTravelDetail.Cost_CostOfAirFare = traveldetail.Cost_CostOfAirFare;
            updateTravelDetail.Cost_PerDiemRate = traveldetail.Cost_PerDiemRate;
            updateTravelDetail.Cost_TotalPerDiem = traveldetail.Cost_TotalPerDiem;
            updateTravelDetail.Cost_RegistrationFee = traveldetail.Cost_RegistrationFee;
            updateTravelDetail.Cost_RentalCar = traveldetail.Cost_RentalCar;
            updateTravelDetail.Cost_Hotel = traveldetail.Cost_Hotel;
            updateTravelDetail.Cost_Miscellaneous = traveldetail.Cost_Miscellaneous;
            updateTravelDetail.Cost_TotalExpense = traveldetail.Cost_TotalExpense;
            updateTravelDetail.Cost_ODCBudgetStatus = traveldetail.Cost_ODCBudgetStatus;
            updateTravelDetail.LastStatus = traveldetail.LastStatus;
            updateTravelDetail.IsProcedural = traveldetail.IsProcedural;
            //_context.Entry(TravelDetailObj).State = System.Data.Entity.EntityState.Modified;
            _context.Entry(updateTravelDetail).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        //public void InsertNotification(TravelNotification travelnotification)
        //{
        //    _context.TravelNotifications.Add(travelnotification);
        //    _context.SaveChanges();
        //}

        public ICollection<TravelDetail> GetAllTravelDetail()
        {
            return _context.TravelDetails.OrderByDescending(x => x.Id).ToList();
        }

        public void AddTravelDetailTrail(TravelDetailTrail objTravelDetailTrailModel)
        {
            _context.TravelDetailTrails.Add(objTravelDetailTrailModel);
            _context.SaveChanges();
            // string Message = Enum.GetName(typeof(TravelRequestStatusEnum), objTravelDetailTrailModel.StatusId);
            string Message = EnumHelper<TravelRequestStatusEnum>.GetDisplayValue((TravelRequestStatusEnum)objTravelDetailTrailModel.StatusId);
            List<int> UserId = new List<int>();
            List<int> data;
            switch (objTravelDetailTrailModel.StatusId)
            {
                case (int)TravelRequestStatusEnum.SubmittedByUser:
                case (int)TravelRequestStatusEnum.Resubmitted:
                    data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.ProjectManager && x.IsActive == 1).Select(e => e.ID).ToList());
                    foreach (var item in data)
                    {
                        UserId.Add(item);
                    }
                    break;
                case (int)TravelRequestStatusEnum.InProcess:
                    UserId.Add(_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.ProjectManager && x.IsActive == 1).Select(e => e.ID).FirstOrDefault());
                    break;
                case (int)TravelRequestStatusEnum.ApprovedByProjectManager:
                    if (objTravelDetailTrailModel.TravelDetail.IsProcedural == (int)ProcedureEnum.DirectFinalStep)
                    {
                        data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.Client2 && x.IsActive == 1).Select(e => e.ID).ToList());
                        foreach (var item in data)
                        {
                            UserId.Add(item);
                        }
                    }
                    else
                    {
                        data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.Client1 && x.IsActive == 1).Select(e => e.ID).ToList());
                        foreach (var item in data)
                        {
                            UserId.Add(item);
                        }
                    }
                    break;
                case (int)TravelRequestStatusEnum.ApprovedByClient1:
                    data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.Client2 && x.IsActive == 1).Select(e => e.ID).ToList());
                    foreach (var item in data)
                    {
                        UserId.Add(item);
                    }
                    break;
                case (int)TravelRequestStatusEnum.ApprovedByClient2:
                    UserId.Add(objTravelDetailTrailModel.TravelDetail.UserID);
                    data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.Client1 && x.IsActive == 1).Select(e => e.ID).ToList());
                    foreach (var item in data)
                    {
                        UserId.Add(item);
                    }
                    data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.ProjectManager && x.IsActive == 1).Select(e => e.ID).ToList());
                    foreach (var item in data)
                    {
                        UserId.Add(item);
                    }
                    break;


                case (int)TravelRequestStatusEnum.RejectedByClient1:
                    UserId.Add(objTravelDetailTrailModel.TravelDetail.UserID);
                    data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.ProjectManager && x.IsActive == 1).Select(e => e.ID).ToList());
                    foreach (var item in data)
                    {
                        UserId.Add(item);
                    }
                    break;

                case (int)TravelRequestStatusEnum.RejectedByClient2:
                    UserId.Add(objTravelDetailTrailModel.TravelDetail.UserID);

                    data = (_context.TravelUsers.Where(x => x.TravelUserRoleMappings.FirstOrDefault().RoleID == (int)UserRoleEnum.ProjectManager && x.IsActive == 1).Select(e => e.ID).ToList());
                    foreach (var item in data)
                    {
                        UserId.Add(item);
                    }
                    break;
                case (int)TravelRequestStatusEnum.RejectedByProjectManager:
                    UserId.Add(objTravelDetailTrailModel.TravelDetail.UserID);
                    break;
            }

            bool Status = true;
            int stepid = objTravelDetailTrailModel.StatusId;
            foreach (int id in UserId)
            {
                InsertNotification(Message, id, Status, stepid, objTravelDetailTrailModel.TravelId, (int)objTravelDetailTrailModel.ProcessedBy);
            }
        }

        public IEnumerable<TravelDetail> GetTravelDetailByUserId(int userid)
        {
            return _context.TravelDetails.Where(x => x.UserID == userid).OrderByDescending(x => x.Id).ToList();
        }
        public IEnumerable<TravelDetail> GetFilteredData(short travelDetailStatus, DateTime fromDate, DateTime toDate, short projectNameCon, string projectName, short PrimeContractorCon, string PrimeContractor, short contractorNumberCon, string contractorNumber, short taskOrderCon, string taskOrder, short CorCon, string CorName, short travellerNameCon, string travellerName, short costCon, decimal cost, short fromState, short toState, short fromCity, short toCity)
        {
            var filteredData = _context.TravelDetails.AsQueryable();

            if (travelDetailStatus != (int)CriteriaForStringEnum.Select)
            {
                if (travelDetailStatus == (int)TravelRequestStatusEnum.SubmittedByUser)
                {
                    filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.SubmittedByUser);
                }
                else if (travelDetailStatus == (int)TravelRequestStatusEnum.Resubmitted)
                {
                    filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.Resubmitted);
                }
                else if (travelDetailStatus == (int)TravelRequestStatusEnum.ApprovedByClient2)
                {
                    filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient2);
                }
                else if (travelDetailStatus == (int)TravelRequestStatusEnum.InProcess)
                {
                    filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByProjectManager || x.LastStatus == (int)TravelRequestStatusEnum.ApprovedByClient1);
                }
                else if (travelDetailStatus == (int)TravelRequestStatusEnum.Rejected)
                {
                    filteredData = filteredData.Where(x => x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient1 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByClient2 || x.LastStatus == (int)TravelRequestStatusEnum.RejectedByProjectManager);
                }

            }
            if (fromDate != DateTime.MinValue)
            {
                filteredData = filteredData.Where(x => x.Summary_SubmittedDate >= fromDate);
            }
            if (toDate != DateTime.MinValue)
            {
                filteredData = filteredData.Where(x => x.Summary_SubmittedDate <= toDate);
            }
            if (projectNameCon != (int)CriteriaForStringEnum.Select)
            {
                if (projectNameCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_ProjectName == projectName);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_ProjectName.Contains(projectName));
                }
            }
            if (PrimeContractorCon != (int)CriteriaForStringEnum.Select)
            {
                if (PrimeContractorCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_PrimeContractor == PrimeContractor);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_PrimeContractor.Contains(PrimeContractor));
                }
            }
            if (CorCon != (int)CriteriaForStringEnum.Select)
            {
                if (CorCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_CorName == CorName);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_CorName.Contains(CorName));
                }
            }
            if (taskOrderCon != (int)CriteriaForStringEnum.Select)
            {
                if (taskOrderCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_TaskOrder == taskOrder);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_TaskOrder.Contains(taskOrder));
                }
            }

            if (travellerNameCon != (int)CriteriaForStringEnum.Select)
            {
                if (travellerNameCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_TravelerName == travellerName);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_TravelerName.Contains(travellerName));
                }
            }
            if (costCon != (int)CriteriaForStringEnum.Select)
            {
                if (costCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Cost_TotalExpense.Value == cost);
                }
                else if (costCon == 2)
                {
                    filteredData = filteredData.Where(x => x.Cost_TotalExpense < cost);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Cost_TotalExpense > cost);
                }
            }

            if (taskOrderCon != (int)CriteriaForStringEnum.Select)
            {
                if (taskOrderCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_TaskOrder == taskOrder);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_TaskOrder.Contains(taskOrder));
                }
            }

            if (taskOrderCon != (int)CriteriaForStringEnum.Select)
            {
                if (taskOrderCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_TaskOrder == taskOrder);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_TaskOrder.Contains(taskOrder));
                }
            }

            if (contractorNumberCon != (int)CriteriaForStringEnum.Select)
            {
                if (taskOrderCon == (int)CriteriaForStringEnum.Is)
                {
                    filteredData = filteredData.Where(x => x.Summary_ContractNumber == contractorNumber);
                }
                else
                {
                    filteredData = filteredData.Where(x => x.Summary_ContractNumber.Contains(contractorNumber));
                }
            }

            if (fromState != (int)CriteriaForStringEnum.Select)
            {
                filteredData = filteredData.Where(x => x.Detail_TravelingFromState == fromState);
            }

            if (toState != (int)CriteriaForStringEnum.Select)
            {
                filteredData = filteredData.Where(x => x.Detail_TravelingToState == toState);
            }

            if (fromCity != (int)CriteriaForStringEnum.Select)
            {
                filteredData = filteredData.Where(x => x.Detail_TravelingFromCity == fromCity);
            }

            if (toCity != (int)CriteriaForStringEnum.Select)
            {
                filteredData = filteredData.Where(x => x.Detail_TravelingToCity == toCity);
            }

            return filteredData.OrderByDescending(x => x.Id);
        }
        public void InsertNotification(string message, int userid, bool status, int stepid, int travelid, int processedby)
        {
            TravelNotification travelnotification = new TravelNotification();
            //travelnotification.Name = "Travel Submit";
            travelnotification.Message = message;
            travelnotification.UserId = userid;
            travelnotification.Status = true;
            travelnotification.StepNotificationId = stepid;
            travelnotification.NotificationDate = DateTime.UtcNow;
            travelnotification.TravelId = travelid;
            travelnotification.ProcessedBy = processedby;
            _context.TravelNotifications.Add(travelnotification);
            _context.SaveChanges();
        }
    }
}
