using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Models
{
    public class Action : BaseModel
    {
        public Action()
        {
            Permissions = new List<Permission>();
        }

        [Key]
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
