using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Common.TravelDataEnum;
using TravelDataRecorder.Database.DAL;

namespace TravelDataRecorder.Database.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TravelDataRecoderEntities _context;
        public UserRepository()
        {
            _context = new TravelDataRecoderEntities();
        }
        public IEnumerable<TravelUser> GetAll()
        {
            return _context.TravelUsers.Where(x => x.IsActive == (int)UserStatusEnum.Active).OrderByDescending(x=>x.ID).ToList();
        }

        public TravelUser GetUserById(int id)
        {
            TravelUser data = _context.TravelUsers.FirstOrDefault(x => x.ID == id);
            if (data != null)
            {
                return data;
            }
            return data;
        }
        public TravelUser GetUserByKey(Guid key)
        {
            TravelUser data = _context.TravelUsers.FirstOrDefault(x => x.GeneratePasswordKey == key);
            if (data != null)
            {
                return data;
            }
            return data;
        }

        public TravelUser Login(String UserName, String Password)
        {
            var data = _context.TravelUsers.FirstOrDefault(x => x.Email == UserName && x.Password == Password && x.IsActive == (int)UserStatusEnum.Active);
            if (data != null)
            {
                return data;
            }
            return data;
        }

        public void UpdateUser(TravelUser traveluser)
        {
            TravelUser UserData = _context.TravelUsers.Find(traveluser.ID);
            if (UserData == null)
            {
                _context.TravelUsers.Add(traveluser);
            }
            else
            {
                UserData.FirstName = traveluser.FirstName;
                UserData.LastName = traveluser.LastName;
                UserData.ContactNumber = traveluser.ContactNumber;
                UserData.ProfileImage = traveluser.ProfileImage;
                UserData.Address = traveluser.Address;
                UserData.OTP = traveluser.OTP;
                UserData.Password = traveluser.Password;

                //_context.Entry(aExists).State = EntityState.Detached;
                //_context.Entry(traveluser).State = EntityState.Modified;

                _context.SaveChanges();

            }

            // _context.Entry(objuser).State = System.Data.Entity.EntityState.Modified;
            //_context.SaveChanges();
        }

        public IQueryable<TravelDetail> GetTravelDataList()
        {
            return _context.TravelDetails.OrderBy(x => x.LastStatus);
        }

        public TravelUser GetUserByEmail(string email)
        {
            var data = _context.TravelUsers.FirstOrDefault(x => x.Email == email && x.IsActive == (int)UserStatusEnum.Active);
            if (data != null)
            {
                return data;
            }
            return data;
        }

        public bool DeleteUser(TravelUser UserData)
        {

            var data = _context.TravelUsers.FirstOrDefault(x => x.ID == UserData.ID);
            if (data != null)
            {
                data.IsActive = 0;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<TravelRole> GetAllRole()
        {
            return _context.TravelRoles.Where(x => x.IsActive == (int)UserStatusEnum.Active).OrderByDescending(x=>x.Id).ToList();
        }

        public List<TravelUser> GetUserListByRole(int roleId)
        {
            return _context.TravelUserRoleMappings.Where(x => x.RoleID == roleId && x.TravelUser.IsActive == (int)UserStatusEnum.Active).Select(x => x.TravelUser).OrderByDescending(x=>x.ID).ToList();
        }
    }
}
