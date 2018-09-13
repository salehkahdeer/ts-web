using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            CreatedOn = DateTime.UtcNow;
        }

        private DateTime createdOn { get; set; }
        public DateTime CreatedOn
        {
            get
            {
                return this.createdOn;
            }
            set
            {
                this.createdOn = value;
                this.ModifiedOn = value;
            }
        }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
