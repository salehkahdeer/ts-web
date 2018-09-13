using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill
{
    internal interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        Task<int> SaveChangesAsync();
        IRepository<T> Repository<T>() where T : class;
    }
}
