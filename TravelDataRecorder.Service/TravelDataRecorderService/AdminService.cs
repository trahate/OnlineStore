using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;
using TravelDataRecorder.Database.Repository;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;
using TravelDataRecorder.Service.ITravelDataRecorderService;

namespace TravelDataRecorder.Service.TravelDataRecorderService
{
    public class AdminService : IAdminService
    {

        private IAdminRepository _adminRepository;
        private IUserRepository _userRepository;
        public AdminService()
        {
            _adminRepository = new AdminRepository();
            _userRepository = new UserRepository();
        }
        // Insert user 
        public UserModel AddUser(TravelUserViewModel traveluser)
        {
            TravelUser UserData = new TravelUser();
            TravelUserRoleMapping UserRoleData = new TravelUserRoleMapping();
            UserData.Email = traveluser.Email;
            UserData.UserName = traveluser.Email;
            UserData.FirstName = traveluser.FirstName;
            UserData.LastName = traveluser.LastName;
            //UserData.Password = "password";
            UserData.IsActive = 1;
            UserRoleData.RoleID = traveluser.RoleId;
            TravelUser data = _adminRepository.AddUser(UserData, UserRoleData);

            return AutoMapper.Mapper.Map<TravelUser, UserModel>(data);
        }

        public bool DeleteUser(UserModel userData)
        {
            TravelUser travelUserData= AutoMapper.Mapper.Map<UserModel, TravelUser>(userData);
            return _userRepository.DeleteUser(travelUserData);
        }

        public List<UserModel> GetAllUser()
        {
            List<TravelUser> UserList = _userRepository.GetAll().ToList();
            return AutoMapper.Mapper.Map<List<TravelUser>, List<UserModel>>(UserList);
        }
        public IEnumerable<TravelRoleModel> GetAllRole()
        {
            IEnumerable<TravelRole> RoleList = _adminRepository.GetAllRole();
            return AutoMapper.Mapper.Map<IEnumerable<TravelRole>, IEnumerable<TravelRoleModel>>(RoleList);
        }
    }
}
