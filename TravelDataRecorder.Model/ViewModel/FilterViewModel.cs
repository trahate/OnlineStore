using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model.ViewModel
{
    public class FilterViewModel
    {
        public int Id { get; set; }
        [Display(Name ="From")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate { get; set; }
        [Display(Name = "To")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ToDate { get; set; }
        public short ProjectNameCriteria { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public short PrimeContractorCriteria { get; set; }
        [Display(Name = "Prime Contractor")]
        public string PrimeContractor { get; set; }
        public short ContractNumberCriteria { get; set; }
        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }
        public short TaskOrderCriteria { get; set; }
        [Display(Name = "Task order")]
        public string TaskOrder { get; set; }
        public short TravelerCostCriteria { get; set; }
        [Display(Name = "Traveller Cost")]
        public decimal? TravelerCost { get; set; }
        [Display(Name = "From State")]
        public short TravelStateFrom { get; set; }
        [Display(Name = "To State")]
        public short TravelStateTo { get; set; }
        [Display(Name = "From City")]
        public short TravelCityFrom { get; set; }
        [Display(Name = "To City")]
        public short TravelCityTo { get; set; }
        [Display(Name = "Status")]
        public short TravelDetailStatus { get; set; }
        public short CORNameCriteria { get; set; }
        public string CORName { get; set; }
        public short TravellerNameCriteria { get; set; }
        public string TravellerName { get; set; }
        public bool IsExport { get; set; }
        public DateTime CreatedOn { get; set; }
        public TravelDetailViewModel TravelDetailViewModel { get; set; }
        public IEnumerable<TravelDetailModel> lstTravelDetailModel { get; set; }

    }
}
