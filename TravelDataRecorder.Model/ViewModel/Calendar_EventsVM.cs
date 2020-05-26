using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model.ViewModel
{
    public class Calendar_EventsVM
    {
        public Int32 id { get; set; }
        public String title { get; set; }
        public String start { get; set; }
        public String end { get; set; }
        public bool allDay { get; set; }
        public String desc { get; set; }
        public string status { get; set; }
        public string color  { get; set; }
        public string className { get; set; }
        public string UserTimezoneOffSet { get; set; }
        
    }
}
