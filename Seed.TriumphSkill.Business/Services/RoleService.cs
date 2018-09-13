using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class RoleService : Service<Role>, IRoleService
    {
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<Permission> permissionRepository;

        public RoleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.roleRepository = unitOfWork.Repository<Role>();
            this.permissionRepository = unitOfWork.Repository<Permission>();
        }

        public IQueryable<Role> All()
        {
            return roleRepository.All();
        }

        public Role GetRoleById(int id)
        {
            return roleRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public Role GetRoleByName(string name, int id)
        {
            return roleRepository.Find(c => c.Name == name && c.ID != id).FirstOrDefault();
        }

        public Permission GetPermissionById(int id)
        {
            return permissionRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public async Task AddAsync(Role role)
        {
            this.roleRepository.Add(role);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            this.roleRepository.Update(role);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = this.roleRepository.Find(c => c.ID == id).FirstOrDefault();
            this.roleRepository.Remove(role);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
