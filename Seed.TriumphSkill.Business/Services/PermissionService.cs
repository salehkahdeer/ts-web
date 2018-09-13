using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class PermissionService : IPermissionService
    {
        private readonly IRepository<Permission> permissionRepository;
        private readonly IRepository<User> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            this.permissionRepository = unitOfWork.Repository<Permission>();
            this.userRepository = unitOfWork.Repository<User>();
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Permission> All()
        {
            return permissionRepository.All();
        }

        public Permission GetPermissionById(int id)
        {
            return permissionRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public async Task AddAsync(Permission permission)
        {
            this.permissionRepository.Add(permission);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Permission permission)
        {
            this.permissionRepository.Update(permission);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var permission = this.permissionRepository.Find(c => c.ID == id).FirstOrDefault();
            this.permissionRepository.Remove(permission);
            await this.unitOfWork.SaveChangesAsync();
        }

        public List<string> GetAllForUser(string username)
        {
            User user = userRepository.FindSingle(u => u.Username == username);
            List<string> permissions = new List<string>();

            if (user.IsAdministrator)
            {
                List<Permission> allPermissions = permissionRepository.All().ToList();

                foreach (Permission permission in allPermissions)
                {
                    foreach (Models.Action action in permission.Actions)
                    {
                        if (!permissions.Contains(action.Name))
                            permissions.Add(action.Name);
                    }
                }
            }
            else
            {
                List<Role> roles = user.Roles.ToList();
                foreach (Role role in roles)
                {
                    foreach (Permission permission in role.Permissions)
                    {
                        foreach (Models.Action action in permission.Actions)
                        {
                            if (!permissions.Contains(action.Name))
                                permissions.Add(action.Name);
                        }
                    }
                }
            }

            return permissions;
        }
    }
}
