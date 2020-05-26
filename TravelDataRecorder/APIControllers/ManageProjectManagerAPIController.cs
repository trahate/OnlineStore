using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TravelDataRecorder.Model;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.APIControllers
{
    public class ManageProjectManagerAPIController : ApiController
    {
        IProjectManagerService _projectManagerRepository;

        public ManageProjectManagerAPIController(IProjectManagerService projectManagerRepository)
        {
            _projectManagerRepository = projectManagerRepository;
        }
      [HttpGet]
        public IEnumerable<TravelDetailModel> GetAllTravelDetail()
        {

            return _projectManagerRepository.GetAllTravelDetail();
        }
        
    }
}
