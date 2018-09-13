using Seed.TriumphSkill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    public interface IDashboardService : IService<Dashboard>
    {
        IQueryable<Dashboard> All();        
        Dashboard GetDashboardById(int id);
        Dashboard GetDashboardByUser(int userId, string name);   
        Task AddAsync(Dashboard dashboard);
        Task UpdateAsync(Dashboard dashboard);
        Task DeleteAsync(int id);
    }
}
