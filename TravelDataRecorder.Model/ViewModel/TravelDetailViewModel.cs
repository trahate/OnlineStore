using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model.ViewModel
{
    public class TravelDetailViewModel
    {
        public int id { get; set; }
        public short IsProcedural { get; set; }
        public short LastStatus { get; set; }
        public string Comments { get; set; }
        public int UserID { get; set; }
        public int StepNotes { get; set; }
        public DateTime? Date { get; set; }
        public int? NextActionRole { get; set; }
        public int RoleId { get; set; }
        public string UserTimezoneOffSet { get; set; }
        public int FromState { get; set; }
        public int ToState { get; set; }
        public int CityName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string criteria { get; set; }
        public int laststepid { get; set; }
        public IEnumerable<TravelCityModel> TravelFromCity { get; set; }
        public IEnumerable<TravelCityModel> TravelToCity { get; set; }
        public IEnumerable<TravelCityModel> TravelReturnCity { get; set; }
        public List<TravelDetailViewModel> lstTraveldetailVM { get; set; }
        public TravelDetailModel traveldetail { get; set; }
        public IEnumerable<TravelStateModel> travelstate { get; set; }
        public IEnumerable<TravelCityModel> travelcity { get; set; }
        public IEnumerable<TravelCityModel> travelCityFrom { get; set; }
        public IEnumerable<TravelCityModel> travelCityTo { get; set; }
    }
}
