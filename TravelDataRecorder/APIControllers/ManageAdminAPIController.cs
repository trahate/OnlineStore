using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.APIControllers
{
    public class ManageAdminAPIController : ApiController
    {
        IAdminService _AdminRepository;
        public ManageAdminAPIController(IAdminService AdminRepository)
        {
            _AdminRepository = AdminRepository;
        }
        [HttpPost]
        public UserModel AddUser(TravelUserViewModel traveluser)
        {
           return _AdminRepository.AddUser(traveluser);
        }

        [HttpGet]
        public HttpResponseMessage GetAllUser()
        {
            IEnumerable<UserModel> result = _AdminRepository.GetAllUser();

            HttpResponseMessage response = new HttpResponseMessage()
            {
                ReasonPhrase = "Success",
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IEnumerable<UserModel>>(result, new JsonMediaTypeFormatter(), "application/json")
            };
            return response;
        }
        [HttpPost]
        public bool DeleteUser(UserModel userdata)
        {
            return _AdminRepository.DeleteUser(userdata);
        }
        [HttpGet]
        public IEnumerable<TravelRoleModel> GetAllRole()
        {
            return _AdminRepository.GetAllRole();
        }
    }
}
