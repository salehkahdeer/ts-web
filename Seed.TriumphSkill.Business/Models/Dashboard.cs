using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Models
{
    public class Dashboard : BaseModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        [ForeignKey("User")]
        public int User_ID { get; set; }
        public virtual User User { get; set; }

        [Column(TypeName="ntext")]
        public string Configuration { get; set; }
    }
}
