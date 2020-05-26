using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
    public class TravelCityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }

        //  public virtual TravelStateModel TravelState { get; set; }
    }
}
