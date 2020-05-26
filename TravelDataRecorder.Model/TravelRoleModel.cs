using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
   public class TravelRoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short IsActive { get; set; }
        public string DisplayName { get; set; }

        //public virtual ICollection<TravelRoleMappingModel> TravelUserRoleMappings { get; set; }
    }
}
