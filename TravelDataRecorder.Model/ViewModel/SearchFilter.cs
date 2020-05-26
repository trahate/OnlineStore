using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model.ViewModel
{
    public class SearchFilter
    {
        public DateTime FromDate { get; set; } = DateTime.UtcNow.AddMonths(-6);
        public DateTime ToDate { get; set; } = DateTime.UtcNow;
        public string Criteria { get; set; } = "all";
    }
}
