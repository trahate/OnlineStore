using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;



namespace TravelDataRecorder.Database.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly TravelDataRecoderEntities _context;
        public AdminRepository()
        {
            _context = new TravelDataRecoderEntities();
        }
      

        public TravelUser AddUser(TravelUser User,TravelUserRoleMapping Role)
        {
            System.Guid guid = System.Guid.NewGuid();
            User.GeneratePasswordKey = guid;
            User.CreatedOn = DateTime.UtcNow;
            _context.TravelUsers.Add(User);
            _context.SaveChanges();
            Role.UserID = User.ID;
            _context.TravelUserRoleMappings.Add(Role);
            _context.SaveChanges();
            return User;
        }
        public IEnumerable<TravelRole> GetAllRole()
        {
            return _context.TravelRoles.OrderByDescending(x=>x.Id);
        }
    }
}
