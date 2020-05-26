using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
    public class TravelDetailModel
    {
        public int Id { get; set; }
        public string Summary_ProjectName { get; set; }
        public string Summary_PrimeContractor { get; set; }
        public string Summary_ContractNumber { get; set; }
        public string Summary_TaskOrder { get; set; }
        public string Summary_CorName { get; set; }
        public DateTime Summary_SubmittedDate { get; set; }
        public string Summary_TravelerName { get; set; }
        public string Detail_AirportDepartingFrom { get; set; }
        public int Detail_TravelingFromCity { get; set; }
        [Display(Name ="Travel State From")]
        public int Detail_TravelingFromState { get; set; }
        [Display(Name = "Travel City From")]
        public int Detail_TravelingToCity { get; set; }
        [Display(Name = "Travel State To")]
        public int Detail_TravelingToState { get; set; }
        [Display(Name = "Travel City To")]
        public int Detail_ReturningToCity { get; set; }
        public int Detail_ReturningToState { get; set; }
        public DateTime Detail_DepartingDate { get; set; }
        public DateTime Detail_ReturningDate { get; set; }
        public decimal? Detail_TotalDays { get; set; }
        public string Detail_TravelPurpose { get; set; }
        public string Detail_TravelSitePOC { get; set; }
        public decimal? Cost_CostOfAirFare { get; set; }
        public decimal? Cost_PerDiemRate { get; set; }
        public decimal? Cost_TotalPerDiem { get; set; }
        public decimal? Cost_RegistrationFee { get; set; }
        public decimal? Cost_RentalCar { get; set; }
        public decimal? Cost_Hotel { get; set; }
        public decimal? Cost_Miscellaneous { get; set; }
        public decimal? Cost_TotalExpense { get; set; }
        public string Cost_ODCBudgetStatus { get; set; }
        public int UserID { get; set; }
        public string UserTimezoneOffSet { get; set; }

        public int LastStatus { get; set; }
        public short IsProcedural { get; set; }
        public string SubmittedDate { get; set; }
        public string DepartingDate { get; set; }
        public string ReturningDate { get; set; }
        public UserModel TravelUser { get; set; }
        public List<TravelDetailTrailModel> TravelDetailTrails { get; set; }
        public TravelStatusModel TravelStatusMaster { get; set; }
        public ICollection<TravelPlaceDetailModel> TravelPlaceDetail { get; set; }
        public virtual TravelCityModel TravelCity { get; set; }
        public virtual TravelCityModel TravelCity1 { get; set; }
        public virtual TravelStateModel TravelState { get; set; }
        public virtual TravelStateModel TravelState1 { get; set; }
        //public ICollection<TravelEmailExceptionModel> TravelEmailExceptions { get; set; }
    }
}
