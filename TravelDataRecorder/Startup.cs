using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Autofac;
using System.Web.Mvc;
using System.Web.Http;
using TravelDataRecorder.Service;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using TravelDataRecorder.App_Start;
using System.Reflection;

[assembly: OwinStartup(typeof(TravelDataRecorder.Startup))]

namespace TravelDataRecorder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            RegisterAutofac();
        }

        private void RegisterAutofac()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(WebApiApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // Register dependencies for web api
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register our Data dependencies
            builder.RegisterModule(new RegisterModule());

            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            AutoMapperConfiguration.Configure();
        }
    }
}
