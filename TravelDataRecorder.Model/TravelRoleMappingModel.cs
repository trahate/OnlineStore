using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
   public class TravelRoleMappingModel
    {
        public int Id { get; set; }
        public int RoleID { get; set; }
        public int UserID { get; set; }

          public virtual TravelRoleModel TravelRole { get; set; }
         //public virtual UserModel TravelUser { get; set; }
    }
}
