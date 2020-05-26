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
    public class CommonService : ICommonService
    {
        private IUserRepository userRepository;
        public CommonService()
        {
            userRepository = new UserRepository();
        }
        public UserModel Login(string UserName,string Password)
        {
            TravelUser User = userRepository.Login(UserName,Password);
            return AutoMapper.Mapper.Map<TravelUser, UserModel>(User);
           
        }
    }
}
