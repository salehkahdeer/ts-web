using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Models
{
    public class Role : BaseModel
    {
        public Role()
        {
            this.Permissions = new List<Permission>();
        }

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual List<Permission> Permissions { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
