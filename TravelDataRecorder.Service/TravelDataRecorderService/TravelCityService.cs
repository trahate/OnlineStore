using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;
using TravelDataRecorder.Database.Repository;
using TravelDataRecorder.Model;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.Service.TravelDataRecorderService
{
   public class TravelCityService: ITravelCityService
    {
        private ITravelCityRepository travelcityrepository;
        public TravelCityService()
        {
            travelcityrepository = new TravelCityRepository();
        }

        public IEnumerable<TravelCityModel> GetAllCity(int StateId)
        {
            IEnumerable<TravelCity> travelcity = travelcityrepository.GetAllCity(StateId);
            return AutoMapper.Mapper.Map<IEnumerable<TravelCity>, IEnumerable<TravelCityModel>>(travelcity);
        }

        public IEnumerable<TravelCityModel> GetCityList()
        {
            IEnumerable<TravelCity> travelcity = travelcityrepository.GetCityList();
            return AutoMapper.Mapper.Map<IEnumerable<TravelCity>, IEnumerable<TravelCityModel>>(travelcity);
        }
    }
}
