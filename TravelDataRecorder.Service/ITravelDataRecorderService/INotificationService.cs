using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Model;

namespace TravelDataRecorder.Service.ITravelDataRecorderService
{
   public interface INotificationService
    {
        IEnumerable<TravelNotificationModel> GetNotification(int userid);
        
             IEnumerable<TravelNotificationModel> GetNotificationList(int userid);
        IEnumerable<TravelNotificationModel> GetNotificationByStatus(int statusid,int userid);

    }
}
