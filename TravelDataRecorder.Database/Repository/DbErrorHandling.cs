using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;

namespace TravelDataRecorder.Database.Repository
{
    public class DbErrorHandling: IDbErrorHandling
    {
        private readonly TravelDataRecoderEntities _context;
        public DbErrorHandling()
        {
            _context = new TravelDataRecoderEntities();
        }

        public void AddEmailException(EmailException objEmailException)
        {

            _context.EmailExceptions.Add(objEmailException);
            _context.SaveChanges();

        }
        public void AddErrorLog(ErrorLog objErrorLog)
        {
            int result = 0;

            _context.ErrorLogs.Add(objErrorLog);
            result = _context.SaveChanges();
        }
    }
}
