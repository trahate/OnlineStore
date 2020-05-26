using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;
namespace TravelDataRecorder.Database.Repository
{
   public interface INotificationRepository
    {
        IEnumerable<TravelNotification> GetNotification(int userid);
        IEnumerable<TravelNotification> GetNotificationList(int userid);
        
            IEnumerable<TravelNotification> GetNotificationByStatus(int statusid, int userid);
    }
}
