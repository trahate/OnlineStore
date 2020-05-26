using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TravelDataRecorder.Model
{
    public class UserModel
    {

        public int ID { get; set; }
        //[Required(ErrorMessage = "Email is Required")]
        //[DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        public string Password { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name ="Email Address")]
        public string Email { get; set; }

        public string Address { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }

        [Display(Name = "Contact Number")]
        public Nullable<long> ContactNumber { get; set; }
        public Nullable<short> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string OTP { get; set; }
        public string ProfileImage { get; set; }
        public Guid GeneratePasswordKey { get; set; }
        public virtual List<TravelRoleMappingModel> TravelUserRoleMappings { get; set; }
    }

    public class CustomUserModel : TokenData
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }

    public class TokenData
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }
}
