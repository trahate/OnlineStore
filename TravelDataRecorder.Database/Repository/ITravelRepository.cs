using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;
namespace TravelDataRecorder.Database.Repository
{
    public interface ITravelRepository
    {
        TravelDetail AddTravelDetail(TravelDetail travelDetail);
        void AddTravelDetailTrail(TravelDetailTrail objTravelDetailTrailModel);

        TravelDetail GetTravelDetail(int id);
        //ICollection<TravelDetail> GetAllTravelDetail(int userId);

        void UpdateTravelDetail(TravelDetail TravelDetailObj);

        void DeleteTravelDetail(TravelDetail TravelDetailObj);

       // void InsertNotification(TravelNotification travelnotification);
        //IEnumerable<TravelState> GetAllState();
        //IEnumerable<TravelCity> GetAllCity(int Id);
        ICollection<TravelDetail> GetAllTravelDetail();
        IEnumerable<TravelDetail> GetTravelDetailByUserId(int id);
        IEnumerable<TravelDetail> GetFilteredData(short travelDetailStatus,DateTime fromDate, DateTime toDate, short projectNameCon,string projectName, short PrimeContractorCon,string PrimeContractor, short contractorNumberCon, string contractorNumber,short taskOrderCon, string taskOrder, short CorCon, string CorName, short travellerNameCon, string travellerName, short costCon, decimal cost,short fromState,short toState,short fromCity,short toCity);
        
    }
}
