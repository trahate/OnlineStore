using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;
using TravelDataRecorder.Model;

namespace TravelDataRecorder.Service.ITravelDataRecorderService
{
    public interface IProjectManagerService
    {
       IEnumerable<TravelDetailModel> GetAllTravelDetail();

    }
}
