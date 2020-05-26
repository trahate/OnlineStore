using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TravelDataRecorder.Common;
using TravelDataRecorder.Model;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using TravelDataRecorder.Common.TravelDataEnum;
using System.Net.Http.Formatting;

namespace TravelDataRecorder.APIControllers
{
    public class ManageUserAPIController : ApiController
    {
        IUserService _userRepository;
        INotificationService _notificationRepository;

        public ManageUserAPIController(IUserService userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public IEnumerable<UserModel> GetAllUser()
        {
            var asd = _userRepository.GetAll();
            return asd;
        }
        [HttpPost]
        public UserModel GetUserById(int id)
        {
            UserModel objUserModel = _userRepository.GetUserById(id);
            return objUserModel;
        }
        [HttpPost]
        public UserModel GetUserByKey(Guid key)
        {
            UserModel objUserModel = _userRepository.GetUserByKey(key);
            return objUserModel;
        }
        [HttpPost]
        public UserModel LogIn(LogInModel UserLogIn)
        {
            UserModel UserData = _userRepository.Login(UserLogIn.UserName, UserLogIn.Password);
            return UserData;
        }
        //Update user profile
        [HttpPut]
        public void UpdateUser(UserModel user)
        {
            _userRepository.UpdateUser(user);
        }
        [HttpPost]
        public HttpResponseMessage GetTravelDataList(int roleId = 0, int userID = 0)
        {
            var allTravelData = _userRepository.GetTravelDataList();
            if (roleId == (int)UserRoleEnum.ProjectManager || (roleId == 0 && userID == 0))   //todo::this condition need to be alter later
            {
                ///return allTravelData;
            }
            else if (roleId == (int)UserRoleEnum.User)
            {
                allTravelData= allTravelData.Where(x => x.UserID == userID);
            }
            else if (roleId == (int)UserRoleEnum.Client1)
            {
                allTravelData = allTravelData.Where(x => x.IsProcedural == (int)ProcedureEnum.Procedural );
            }
            else
            {
                allTravelData = allTravelData.Where(x => (x.IsProcedural == (int)ProcedureEnum.Procedural || x.IsProcedural == (int)ProcedureEnum.DirectFinalStep));
            }
            HttpResponseMessage response = new HttpResponseMessage()
            {
                ReasonPhrase = "Success",
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IEnumerable<TravelDetailModel>>(allTravelData, new JsonMediaTypeFormatter(), "application/json")
            };
            return response;
        }
        [HttpPost]
        public UserModel GetUserByEmail(string email)
        {
            UserModel UserData = _userRepository.GetUserByEmail(email);
            return UserData;
        }
        [HttpGet]
        [Route("getallrole")]
        public HttpResponseMessage GetAllRole()
        {
            List<TravelRoleModel> result = _userRepository.GetAllRole();
            HttpResponseMessage response = new HttpResponseMessage()
            {
                ReasonPhrase = "Success",
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<List<TravelRoleModel>>(result, new JsonMediaTypeFormatter(), "application/json")
            };
            return response;
        }


        [HttpGet]
        public List<UserModel> GetUserListByRole(int roleId)
        {
            List<UserModel> UserData = _userRepository.GetUserListByRole(roleId);
            return UserData;
        }
    }
}
