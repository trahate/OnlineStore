using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Model;

namespace TravelDataRecorder.Service.ITravelDataRecorderService
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        UserModel GetUserById(int id);
        UserModel GetUserByKey(Guid key);
        UserModel Login(string UserName, string Password);
        void UpdateUser(UserModel user);
        IEnumerable<TravelDetailModel> GetTravelDataList();
        UserModel GetUserByEmail(string email);
        List<TravelRoleModel> GetAllRole();
        List<UserModel> GetUserListByRole(int roleId);
    }
}
