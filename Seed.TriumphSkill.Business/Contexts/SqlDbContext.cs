using Seed.TriumphSkill.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Seed.TriumphSkill.Contexts
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext()
            : base("SqlContext")
        {
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Models.Action> Actions { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        public DbSet<Village> Villages { get; set; }
    }
}
