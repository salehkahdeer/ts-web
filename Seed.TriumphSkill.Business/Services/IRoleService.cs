using Seed.TriumphSkill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    public interface IRoleService : IService<Role>
    {
        IQueryable<Role> All();        
        Role GetRoleById(int id);
        Role GetRoleByName(string name, int id);
        Permission GetPermissionById(int id);        
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}
