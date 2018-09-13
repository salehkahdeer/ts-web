using Seed.TriumphSkill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    public interface IMenuService
    {
        IQueryable<Menu> All();
        IQueryable<Menu> GetAllFirstLevel();
        List<Menu> GetAllForUser(string username);
        Menu GetMenuById(int id);        
        Task AddAsync(Menu menu);
        Task UpdateAsync(Menu menu);
        Task DeleteAsync(int id);
    }
}
