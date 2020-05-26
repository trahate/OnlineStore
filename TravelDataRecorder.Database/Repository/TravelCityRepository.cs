using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;

namespace TravelDataRecorder.Database.Repository
{
    public class TravelCityRepository : ITravelCityRepository
    {
        private readonly TravelDataRecoderEntities _context;
        public TravelCityRepository()
        {
            _context = new TravelDataRecoderEntities();
        }
        public IEnumerable<TravelCity> GetAllCity(int StateId)
        {
            var data = _context.TravelCities.Where(x => x.StateId == StateId).OrderByDescending(x=>x.Name).ToList();
            if (data != null)
            {
                return data;
            }
            return data;
        }

        public IEnumerable<TravelCity> GetCityList()
        {
            return _context.TravelCities.ToList();
        }
    }
}
