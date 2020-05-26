using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
   public class TravelErrorLogModel
    {
        public string ErrorInnerException { get; set; }
        public string ErrorInnerExceptionMessage { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ErrorTimeStamp { get; set; }
        public string StackTrace { get; set; }
    }
}
