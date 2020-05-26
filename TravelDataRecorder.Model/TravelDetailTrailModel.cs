using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
    public class TravelDetailTrailModel
    {
        public int Id { get; set; }
        public int TravelId { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> ProcessedBy { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string UserTimezoneOffSet { get; set; }
        public string Comments { get; set; }
        public Nullable<int> StepNotes { get; set; }
        public Nullable<int> NextActionRole { get; set; }
      //  public TravelDetailModel TravelDetail { get; set; }
        public virtual TravelStatusModel TravelStatusMaster { get; set; }

    }
}
