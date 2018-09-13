using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Seed.TriumphSkill.Models
{
    public class Menu : BaseModel
    {
        public Menu()
        {
            this.Children = new List<Menu>();
        }

        [Key]
        public int ID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }
                
        [Display(Name = "Route")]
        public string Route
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Controller))
                    return null;
                else
                    return string.Format("{0}.{1}.{2}", "ACTION", Controller, Action);
            }
        }

        [StringLength(100)]
        public string Action { get; set; }

        [StringLength(100)]
        public string Controller { get; set; }

        public int Order { get; set; }

        public int? ParentID { get; set; }
        [JsonIgnore]
        public virtual Menu Parent { get; set; }

        public virtual List<Menu> Children { get; set; }
    }
}
