using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDataRecorder.Database.DAL;
using TravelDataRecorder.Model;

namespace TravelDataRecorder.Service
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<UserModel, TravelUser>().ReverseMap();
            Mapper.CreateMap<TravelCityModel, TravelCity>().ReverseMap();
            Mapper.CreateMap<TravelCountryModel, TravelCountry>().ReverseMap();
            Mapper.CreateMap<TravelEmailExceptionModel, TravelEmailException>().ReverseMap();
            Mapper.CreateMap<TravelErrorLogModel, TravelErrorLog>().ReverseMap();
            Mapper.CreateMap<TravelNotificationModel, TravelNotification>().ReverseMap();
            Mapper.CreateMap<TravelRoleMappingModel, TravelUserRoleMapping>().ReverseMap();
            Mapper.CreateMap<TravelRoleModel, TravelRole>().ReverseMap();
            Mapper.CreateMap<TravelStateModel, TravelState>().ReverseMap();
            Mapper.CreateMap<TravelDetailModel, TravelDetail>().ReverseMap();
            Mapper.CreateMap<TravelDetailTrailModel, TravelDetailTrail>().ReverseMap();
            Mapper.CreateMap<TravelStatusModel, TravelStatusMaster>().ReverseMap();
            Mapper.CreateMap<TravelPlaceDetailModel, TravelPlaceDetail>().ReverseMap();
            Mapper.CreateMap<EmailExceptionModel, EmailException>().ReverseMap();
            Mapper.CreateMap<ErrorLogModel, ErrorLog>().ReverseMap();
        }
    }
}
