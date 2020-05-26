using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;

namespace TravelDataRecorder.Database.Repository
{
    
    public class TravelStateRepository:ITravelStateRepository
    {
        private readonly TravelDataRecoderEntities _context;
        public TravelStateRepository()
        {
            _context = new TravelDataRecoderEntities();
        }
        public IEnumerable<TravelState> GetAllStates()
        {
            return _context.TravelStates.OrderBy(x=>x.Name);
        }
    }
}
