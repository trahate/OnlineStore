using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
namespace TravelDataRecorder.Service.ITravelDataRecorderService
{
    public interface ITravelService
    {
        TravelDetailModel AddTravelDetail(TravelDetailModel TravelDetail);// Insert travel form

        void AddTravelDetailTrail(TravelDetailTrailModel objTravelDetailTrailModel);

        TravelDetailModel GetTravelDetail(int id);
        IEnumerable<TravelDetailModel> GetTravelDetailByUserId(int userid);

        void UpdateTravelDetail(TravelDetailModel TravelDetail);

        void DeleteTravelDetail(TravelDetailModel TravelDetail);
        IEnumerable<TravelDetailModel> GetFilteredData(FilterViewModel objFilterViewModel);

        //IEnumerable<TravelStateModel> GetAllStates();
        //IEnumerable<CityModel> GetAllCity(int Id);
    }
}
