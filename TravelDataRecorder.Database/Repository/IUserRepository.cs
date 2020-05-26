using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;

namespace TravelDataRecorder.Database.Repository
{
    public interface IUserRepository
    {
        IEnumerable<TravelUser> GetAll();
        
        TravelUser Login(String UserName, String Password);
        TravelUser GetUserById(int id);
        TravelUser GetUserByKey(Guid Key);
        void UpdateUser(TravelUser user);
        IQueryable<TravelDetail> GetTravelDataList();
        TravelUser GetUserByEmail(string email);
        bool DeleteUser(TravelUser UserData);
        List<TravelRole> GetAllRole();
        List<TravelUser> GetUserListByRole(int roleId);

    }
}
