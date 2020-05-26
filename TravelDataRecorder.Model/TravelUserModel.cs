using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
    public class TravelUserModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long? ContactNumber { get; set; }
        public short? IsActive { get; set; }
        public int? ParentId { get; set; }
        public string Address { get; set; }
        public string OTP { get; set; }
        public Nullable<System.Guid> GeneratePasswordKey { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public virtual List<TravelRoleMappingModel> TravelUserRoleMappings { get; set; }
        public List<TravelDetailModel> TravelDetails { get; set; }
        public List<TravelNotificationModel> TravelNotifications { get; set; }
    }
}
