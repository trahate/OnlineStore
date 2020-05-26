using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Model;

namespace TravelDataRecorder.Service.ITravelDataRecorderService
{
   public interface ITravelCityService
    {
        IEnumerable<TravelCityModel> GetAllCity(int StateId);
        IEnumerable<TravelCityModel> GetCityList(); 
    }
}
