using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelDataRecorder.Service.ITravelDataRecorderService;
using TravelDataRecorder.Service.TravelDataRecorderService;

namespace TravelDataRecorder.App_Start
{
    public class RegisterModule : Module
    {
        public RegisterModule()
        {
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<AdminService>().As<IAdminService>().InstancePerRequest();
            builder.RegisterType<TravelService>().As<ITravelService>().InstancePerRequest();
            builder.RegisterType<TravelStateService>().As<ITravelStateService>().InstancePerRequest();
            builder.RegisterType<TravelCityService>().As<ITravelCityService>().InstancePerRequest();
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerRequest();
            builder.RegisterType<ProjectManagerService>().As<IProjectManagerService>().InstancePerRequest();
            builder.RegisterType<DbErrorHandlingService>().As<IDbErrorHandlingService>().InstancePerRequest();
            base.Load(builder);
        }
    }
}