using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Seed.TriumphSkill.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Seed.TriumphSkill
{
    public class AutofacConfig
    {
        public static void ConfigureContainer(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);   

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            builder.Register<String>(c => Guid.NewGuid().ToString())
                .Named<String>("correlationId")
                .InstancePerRequest();

            // Register our Data dependencies
            builder.RegisterModule(new DataModule());

            //Register our Service dependencies
            builder.RegisterModule(new ServiceModule());

            builder.RegisterHttpRequestMessage(config);
            builder.RegisterFilterProvider();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


        }
    }
}