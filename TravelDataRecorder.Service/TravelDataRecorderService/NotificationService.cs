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
    public class NotificationService:INotificationService
    {
        private INotificationRepository notificationRepository;
        public NotificationService()
        {
            notificationRepository = new NotificationRepository();
        }

       
        public IEnumerable<TravelNotificationModel> GetNotification(int userid)
        {
            IEnumerable <TravelNotification> notification = notificationRepository.GetNotification(userid);
            return AutoMapper.Mapper.Map<IEnumerable<TravelNotification>, IEnumerable<TravelNotificationModel>>(notification);
        }

        public IEnumerable<TravelNotificationModel> GetNotificationByStatus(int statusid,int userid)
        {
            IEnumerable<TravelNotification> notification = notificationRepository.GetNotificationByStatus(statusid,userid);
            return AutoMapper.Mapper.Map<IEnumerable<TravelNotification>, IEnumerable<TravelNotificationModel>>(notification);
        }

        public IEnumerable<TravelNotificationModel> GetNotificationList(int userid)
        {
            IEnumerable<TravelNotification> notificationlist = notificationRepository.GetNotificationList(userid);
            return AutoMapper.Mapper.Map<IEnumerable<TravelNotification>, IEnumerable<TravelNotificationModel>>(notificationlist);
        }
    }
}
