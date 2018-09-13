using Seed.TriumphSkill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    public interface IVillageService : IService<Village>
    {
        IQueryable<Village> All();        
        Village GetVillageById(int id);        
        Task AddAsync(Village village);
        Task UpdateAsync(Village village);
        Task DeleteAsync(int id);
    }
}
