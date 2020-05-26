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
    public class DbErrorHandlingService : IDbErrorHandlingService
    {
        DbErrorHandling _dbErrorHandling = new DbErrorHandling();
        public void AddEmailException(EmailExceptionModel objEmailExceptionModel)
        {
            var objEmailException = AutoMapper.Mapper.Map<EmailExceptionModel, EmailException>(objEmailExceptionModel);
            _dbErrorHandling.AddEmailException(objEmailException);
        }
        public void AddErrorLog(ErrorLogModel objErrorModel)
        {
            var objError = AutoMapper.Mapper.Map<ErrorLogModel, ErrorLog>(objErrorModel);
            _dbErrorHandling.AddErrorLog(objError);
        }
    }
}
