using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model.ViewModel
{
    public class DashboardViewModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int TotalUser { get; set; }
        public int Traveller { get; set; }
        public int ProjectOfficer { get; set; }
        public int ChiefEndUser { get; set; }
        public int Approved { get; set; }
        public int Reject { get; set; }
        public int InProgress { get; set; }
        public int Submitted { get; set; }
        public int TotalTravel { get; set; }
        public int New { get; set; }
        public int Pending { get; set; }
        public int ReSubmitted { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


    }
}
