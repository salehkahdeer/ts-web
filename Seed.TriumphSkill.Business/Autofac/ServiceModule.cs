using Autofac;
using Seed.TriumphSkill.Contexts;
using Seed.TriumphSkill.Repositories;
using Seed.TriumphSkill.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Autofac
{
    public class ServiceModule : Module
    {
        public ServiceModule()
        {
            
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(LoggerFacade<>)).As(typeof(ILoggerFacade<>))
                    .WithParameter((p, c) => p.Name == "correlationId", (p, c) => c.ResolveNamed<string>("correlationId"));

            builder.RegisterType<MenuService>().As<IMenuService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<PermissionService>().As<IPermissionService>();

            builder.RegisterType<SystemSettingService>().As<ISystemSettingService>();
            builder.RegisterType<DashboardService>().As<IDashboardService>();

            builder.RegisterType<VillageService>().As<IVillageService>();

            base.Load(builder);
        }
    }
}
