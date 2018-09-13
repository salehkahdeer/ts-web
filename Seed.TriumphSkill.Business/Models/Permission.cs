using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Models
{
    public class Permission : BaseModel
    {
        public Permission()
        {
            this.Actions = new List<Action>();
            this.Roles = new List<Role>();
        }

        [Key]
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Feature")]
        public string Feature { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
