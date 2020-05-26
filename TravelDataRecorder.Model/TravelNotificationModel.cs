using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
    public class TravelNotificationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public UserModel TravelUser { get; set; }
        public bool Status { get; set; }
        public int StepNotificationId { get; set; }
        public DateTime NotificationDate { get; set; }
        public int TravelId { get; set; }
        public int ProcessedBy { get; set; }

        public UserModel TravelUser1 { get; set; }
    }
}
