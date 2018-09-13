using Autofac;
using Seed.TriumphSkill.Contexts;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Autofac
{
    public class DataModule : Module
    {
        public DataModule()
        {
            
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new SqlDbContext()).
                             As<DbContext>();

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            base.Load(builder);
        }
    }
}
