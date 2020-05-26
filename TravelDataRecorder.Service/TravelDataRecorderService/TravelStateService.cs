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
   public class TravelStateService:ITravelStateService
    {
        private ITravelStateRepository TravelStateRepository;
       
        public TravelStateService()
        {
            TravelStateRepository = new TravelStateRepository();
        }

        public IEnumerable<TravelStateModel> GetAllStates()
        {
           IEnumerable<TravelState> lstStates = TravelStateRepository.GetAllStates();
            return AutoMapper.Mapper.Map<IEnumerable<TravelState>, IEnumerable<TravelStateModel>>(lstStates);
        }
      
    }
}
