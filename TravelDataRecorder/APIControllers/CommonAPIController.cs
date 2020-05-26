using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using TravelDataRecorder.Model;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.APIControllers
{
    public class CommonAPIController : ApiController
    {
        ITravelStateService _TravelStateRepository;
        ITravelCityService _TravelCityRepository;
        INotificationService _NotificationRepository;
        public CommonAPIController(ITravelStateService TravelStateService, ITravelCityService TravelCityService, INotificationService NotificationService)
        {
            _TravelStateRepository = TravelStateService;
            _TravelCityRepository = TravelCityService;
            _NotificationRepository = NotificationService;
        }

        [HttpGet]
        public IEnumerable<TravelStateModel> GetAllStates()
        {
            var states = _TravelStateRepository.GetAllStates();
            return states;
        }
      
        [HttpPost]
        public IEnumerable<TravelCityModel> GetallCity(int Id)
        {
            var Cities = _TravelCityRepository.GetAllCity(Id);
            return Cities;
        }
        [HttpGet]
        public IEnumerable<TravelCityModel> GetCityList()
        {
            var Cities = _TravelCityRepository.GetCityList();
            return Cities;
        }
        [HttpPost]
        public IEnumerable<TravelNotificationModel> GetNotification(int userid)
        {
            IEnumerable <TravelNotificationModel> travelnotification = _NotificationRepository.GetNotification(userid);
            return travelnotification;
    }
        public  HttpResponseMessage GetNotificationList(int userid,string status)
        {
            IEnumerable<TravelNotificationModel> travelnotification = _NotificationRepository.GetNotificationList(userid).Where(m=>m.StepNotificationId!=0);
            if (status.Trim() != "0")
            {
                var array = status.Split(',').ToArray();
                travelnotification = travelnotification.Where(m => array.Contains(m.StepNotificationId.ToString())&& m.Status==true);
                foreach (var item in array)
                {
                    GetNotificationByStatus(Convert.ToInt32(item), userid);
                }
              
            }
            HttpResponseMessage response = new HttpResponseMessage()
            {
                ReasonPhrase = "Success",
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IEnumerable<TravelNotificationModel>>(travelnotification, new JsonMediaTypeFormatter(), "application/json")
            };
            return response;
        }

             public IEnumerable<TravelNotificationModel> GetNotificationByStatus(int statusid,int userid)
        {
            IEnumerable<TravelNotificationModel> travelnotification = _NotificationRepository.GetNotificationByStatus(statusid, userid);
            return travelnotification;
        }
    }
}
