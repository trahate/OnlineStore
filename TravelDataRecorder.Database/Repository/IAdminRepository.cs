﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;


namespace TravelDataRecorder.Database.Repository
{
    public interface IAdminRepository
    {
        TravelUser AddUser(TravelUser user,TravelUserRoleMapping role);
        IEnumerable<TravelRole> GetAllRole();
    }
}
