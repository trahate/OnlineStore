using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
    public class EmailExceptionModel
    {
        public int Id { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailSubject { get; set; }
        public string MailMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorInnerException { get; set; }
        public string ErrorInnerExceptionMessage { get; set; }
    }
}
