using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class MenuService : IMenuService
    {
        private readonly IRepository<Menu> menuRepository;
        private readonly IRepository<User> userRepository;        
        private readonly IPermissionService permissionService;
        private readonly IUnitOfWork unitOfWork;

        public MenuService(IUnitOfWork unitOfWork,
            IPermissionService permissionService)
        {
            this.menuRepository = unitOfWork.Repository<Menu>();
            this.userRepository = unitOfWork.Repository<User>();
            this.permissionService = permissionService;
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<Menu> All()
        {
            return menuRepository.All();
        }

        public IQueryable<Menu> GetAllFirstLevel()
        {
            return menuRepository.Find(m => m.Parent == null);
        }

        public List<Menu> GetAllForUser(string username)
        {
            User user = userRepository.FindSingle(u => u.Username == username);
            List<Menu> menus = menuRepository.Find(m => m.Parent == null).OrderBy(m => m.Order).ToList();
            List<string> permissions = permissionService.GetAllForUser(user.Username);

            for (int i = menus.Count - 1; i >= 0; i--)
            {
                if (menus[i].Route != null)
                {
                    if (!permissions.Contains(menus[i].Route))
                        menus.RemoveAt(i);
                }
                else
                {
                    List<Menu> childMenus = menus[i].Children.ToList();
                    for (int j = childMenus.Count - 1; j >= 0; j--)
                    {
                        if (childMenus[j].Route != null)
                        {
                            if (!permissions.Contains(childMenus[j].Route))
                                menus[i].Children.Remove(childMenus[j]);
                        }
                    }

                    if (menus[i].Children.Count == 0)
                        menus.RemoveAt(i);
                }
            }

            return menus;
        }

        public Menu GetMenuById(int id)
        {
            return menuRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public async Task AddAsync(Menu menu)
        {
            this.menuRepository.Add(menu);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Menu menu)
        {
            this.menuRepository.Update(menu);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var menu = this.menuRepository.Find(c => c.ID == id).FirstOrDefault();
            this.menuRepository.Remove(menu);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
