using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seed.TriumphSkill.ViewModels
{
    public class Role
    {
        public Role()
        {
            RoleInfo = new Models.Role();
            this.Permissions = new List<Permission>();
        }

        public Models.Role RoleInfo { get; set; }
        public List<Permission> Permissions { get; set; }


        public bool IsSelected { get; set; }
        public static List<Role> ConvertAll(List<Models.Role> roles)
        {
            return roles.ConvertAll(r => new Role { RoleInfo = r });
        }
    }
}