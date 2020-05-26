using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TravelDataRecorder.Model
{
    public class LogInModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid UserName")]
       
        public String UserName { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
