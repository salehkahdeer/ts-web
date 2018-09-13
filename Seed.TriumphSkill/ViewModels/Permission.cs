using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seed.TriumphSkill.ViewModels
{
    public class Permission
    {
        public Permission()
        {
            PermissionInfo = new Models.Permission();
        }

        public Models.Permission PermissionInfo { get; set; }
        public bool IsSelected { get; set; }

        public static List<Permission> ConvertAll(List<Models.Permission> permissions)
        {
            return permissions.ConvertAll(p => new Permission { PermissionInfo = p });
        }
    }
}