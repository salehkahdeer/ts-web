using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Models
{
    public class User : BaseModel
    {
        public User()
        {
            this.Roles = new List<Role>();
            this.Dashboards = new List<Dashboard>();
        }

        [Key]
        public int ID { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DefaultValue(true)]
        public bool IsAdministrator { get; set; }

        public virtual List<Role> Roles { get; set; }

        public virtual List<Dashboard> Dashboards { get; set; }
    }
}
