using Seed.TriumphSkill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    public interface IUserService : IService<User>
    {
        IQueryable<User> All();        
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User GetUserByUsername(string username, int id);
        Role GetRoleById(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
