using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class UserService : Service<User>, IUserService
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Role> roleRepository;

        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.userRepository = unitOfWork.Repository<User>();
            this.roleRepository = unitOfWork.Repository<Role>();
        }

        public IQueryable<User> All()
        {
            return userRepository.All();
        }

        public User GetUserById(int id)
        {
            return userRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return userRepository.Find(c => c.Username == username).FirstOrDefault();
        }

        public User GetUserByUsername(string username, int id)
        {
            return userRepository.Find(c => c.Username == username && c.ID != id).FirstOrDefault();
        }

        public Role GetRoleById(int id)
        {
            return roleRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public async Task AddAsync(User user)
        {
            this.userRepository.Add(user);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            this.userRepository.Update(user);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = this.userRepository.Find(c => c.ID == id).FirstOrDefault();
            this.userRepository.Remove(user);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
