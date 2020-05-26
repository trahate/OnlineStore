using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
   public class TravelEmailExceptionModel
    {
        public int Id { get; set; }
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public int TravelId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<short> status { get; set; }

       // public virtual TravelDetailModel TravelDetail { get; set; }
    }
}
