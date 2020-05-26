using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;
using TravelDataRecorder.Database.Repository;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.Service.TravelDataRecorderService
{
    public class TravelService : ITravelService
    {
        private ITravelRepository TravelRepository;
        public TravelService()
        {
            TravelRepository = new TravelRepository();
        }


        public TravelDetailModel AddTravelDetail(TravelDetailModel travelDetailData)
        {
            TravelDetail travelDetail = AutoMapper.Mapper.Map<TravelDetailModel, TravelDetail>(travelDetailData);
            travelDetail.TravelDetailTrails.Add(new TravelDetailTrail()
            {
                TravelId = travelDetail.Id,
                StatusId = 1,
                ProcessedBy = travelDetail.UserID,
                Date = travelDetail.Summary_SubmittedDate,

            });

            //foreach (var travelPlace in travelDetailData.TravelPlaceDetail)
            //{
            //    travelDetail.TravelPlaceDetails.Add(new Database.DAL.TravelPlaceDetail()
            //    {
            //        Id = travelDetail.Id,
            //        StateId = travelDetailData.Detail_ReturningToState,
            //        CityId = travelDetailData.Detail_ReturningToCity,
            //        Type="TravellingFrom"
            //    });
            //}

            TravelDetail traveldata = TravelRepository.AddTravelDetail(travelDetail);
            
            return AutoMapper.Mapper.Map<TravelDetail, TravelDetailModel>(traveldata);
        }

        public void AddTravelDetailTrail(TravelDetailTrailModel objTravelDetailTrailVM)
        {
            TravelDetailTrail objTrail = AutoMapper.Mapper.Map<TravelDetailTrailModel, TravelDetailTrail>(objTravelDetailTrailVM);
            TravelRepository.AddTravelDetailTrail(objTrail);
        }

        public void DeleteTravelDetail(TravelDetailModel TravelDetail)
        {
            var Traveldata = AutoMapper.Mapper.Map<TravelDetailModel, TravelDetail>(TravelDetail);
            TravelRepository.DeleteTravelDetail(Traveldata);
        }

        public TravelDetailModel GetTravelDetail(int id)
        {
            var TraveldataDetail = TravelRepository.GetTravelDetail(id);

            return AutoMapper.Mapper.Map<TravelDetail, TravelDetailModel>(TraveldataDetail);
        }

        public IEnumerable<TravelDetailModel> GetTravelDetailByUserId(int userid)
        {
            IEnumerable<TravelDetail> traveldata = TravelRepository.GetTravelDetailByUserId(userid);

            return AutoMapper.Mapper.Map<IEnumerable<TravelDetail>, IEnumerable<TravelDetailModel>>(traveldata);
        }

        public void UpdateTravelDetail(TravelDetailModel traveldetail)
        {
            TravelDetail traveldata = AutoMapper.Mapper.Map<TravelDetailModel, TravelDetail>(traveldetail);
            TravelRepository.UpdateTravelDetail(traveldata);
        }

        public IEnumerable<TravelDetailModel> GetFilteredData(FilterViewModel objFilterVM)
        {
            var allTravelData = TravelRepository.GetFilteredData(objFilterVM.TravelDetailStatus, objFilterVM.FromDate, objFilterVM.ToDate, objFilterVM.ProjectNameCriteria, objFilterVM.ProjectName, objFilterVM.PrimeContractorCriteria, objFilterVM.PrimeContractor, objFilterVM.ContractNumberCriteria, objFilterVM.ContractNumber, objFilterVM.TaskOrderCriteria, objFilterVM.TaskOrder, objFilterVM.CORNameCriteria, objFilterVM.CORName, objFilterVM.TravellerNameCriteria, objFilterVM.TravellerName, objFilterVM.TravelerCostCriteria, objFilterVM.TravelerCost.Value, objFilterVM.TravelStateFrom, objFilterVM.TravelStateTo, objFilterVM.TravelCityFrom, objFilterVM.TravelCityTo);

            return AutoMapper.Mapper.Map<IEnumerable<TravelDetail>, IEnumerable<TravelDetailModel>>(allTravelData);

        }

    }
}
