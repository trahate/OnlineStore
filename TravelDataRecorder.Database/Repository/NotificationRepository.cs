using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Common.TravelDataEnum;
using TravelDataRecorder.Database.DAL;
namespace TravelDataRecorder.Database.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly TravelDataRecoderEntities _context;
        public NotificationRepository()
        {
            _context = new TravelDataRecoderEntities();
        }
        public IEnumerable<TravelNotification> GetNotification(int userid)
        {
            IEnumerable<TravelNotification> data = _context.TravelNotifications.Where(x => x.UserId == userid && x.Status == true).OrderByDescending(x => x.Id).ToList();
            if (data != null)
            {
                return data;
            }
            return data;
        }

        public IEnumerable<TravelNotification> GetNotificationByStatus(int statusid, int userid)
        {
            IEnumerable<TravelNotification> travelnotification = _context.TravelNotifications.Where(x => x.StepNotificationId == statusid && x.UserId == userid && x.Status == true).ToList().OrderByDescending(x => x.NotificationDate);
            foreach (TravelNotification notification in travelnotification)
            {
                notification.Status = false;
                _context.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            }

            _context.SaveChanges();

            return travelnotification;
        }

        public IEnumerable<TravelNotification> GetNotificationList(int userid)
        {
            IEnumerable<TravelNotification> data = _context.TravelNotifications.Where(x => x.UserId == userid).OrderByDescending(x=>x.Id).ToList();
            if (data != null)
            {
                return data;
            }
            return data;
        }
    }
}
