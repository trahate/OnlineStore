using AutoMapper;
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
   public class ProjectManagerService : IProjectManagerService
    {
        private ITravelRepository TravelRepository;
        
        public ProjectManagerService()
        {
            TravelRepository = new TravelRepository();
            
        }

        public IEnumerable<TravelDetailModel> GetAllTravelDetail()
        {
            IEnumerable<TravelDetail> data = TravelRepository.GetAllTravelDetail();
            return Mapper.Map<IEnumerable<TravelDetail>, IEnumerable<TravelDetailModel>>(data);

        }

        
    }
}
