using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TravelDataRecorder.Model.ViewModel
{
    public class TravelUserPasswordViewModel
    {
        public int Id { get; set; }
        
        [Display(Name ="Old Password")]
        public string OldPassword { get; set; }

        
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
