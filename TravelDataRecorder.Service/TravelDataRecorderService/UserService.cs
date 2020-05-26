using AutoMapper;
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
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        public UserService()
        {
            userRepository = new UserRepository();
        }
        public IEnumerable<UserModel> GetAll()
        {
            IEnumerable<TravelUser> lstUsers = userRepository.GetAll();
            return Mapper.Map<IEnumerable<TravelUser>, IEnumerable<UserModel>>(lstUsers);
        }

        public UserModel GetUserById(int id)
        {
            TravelUser User = userRepository.GetUserById(id);
            return Mapper.Map<TravelUser, UserModel>(User);
        }
        public UserModel GetUserByKey(Guid key)
        {
            TravelUser User = userRepository.GetUserByKey(key);
            return Mapper.Map<TravelUser, UserModel>(User);
        }

        public UserModel Login(string UserName, string Password)
        {
            TravelUser User = userRepository.Login(UserName, Password);
            return Mapper.Map<TravelUser, UserModel>(User);
        }

        public void UpdateUser(UserModel user)
        {
            TravelUser Userdata = Mapper.Map<UserModel, TravelUser>(user);
            userRepository.UpdateUser(Userdata);
        }

        public IEnumerable<TravelDetailModel> GetTravelDataList()
        {
            var lstTravelDetail = userRepository.GetTravelDataList();

            return Mapper.Map<IEnumerable<TravelDetail>, IEnumerable<TravelDetailModel>>(lstTravelDetail);
        }

        public UserModel GetUserByEmail(string email)
        {
            TravelUser User = userRepository.GetUserByEmail(email);
            return Mapper.Map<TravelUser, UserModel>(User);
        }

        public List<TravelRoleModel> GetAllRole()
        {
            List < TravelRole> RoleList= userRepository.GetAllRole();
            return Mapper.Map<List<TravelRole>, List<TravelRoleModel>>(RoleList);
        }


        public List<UserModel> GetUserListByRole(int roleId)
        {
            List<TravelUser> roleList = userRepository.GetUserListByRole(roleId);
            return Mapper.Map<List<TravelUser>, List<UserModel>>(roleList);
        }
        
    }
}
