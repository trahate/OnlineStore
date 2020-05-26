using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Model;
using TravelDataRecorder.Model.ViewModel;

namespace TravelDataRecorder.Service.ITravelDataRecorderService
{
    public interface IAdminService
    {
        UserModel AddUser(TravelUserViewModel traveluser);
        List<UserModel> GetAllUser();
        bool DeleteUser(UserModel userData);
        IEnumerable<TravelRoleModel> GetAllRole();
    }
}
