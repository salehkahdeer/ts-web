using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Models
{
    public class SystemSetting : BaseModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string SettingTypeValue { get; set; }

        [Display(Name = "Setting Type")]
        public SettingType SettingType
        {
            get
            {
                if (SettingTypeValue != null)
                {
                    return (SettingType)Enum.Parse(typeof(SettingType), SettingTypeValue);
                }
                else
                {
                    return Enum.GetValues(typeof(SettingType)).Cast<SettingType>().First();
                }
            }
            set
            {
                SettingTypeValue = value.ToString();
            }
        }

        [MaxLength(128, ErrorMessage = "Length cannot exceed 128 characters.")]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        [MaxLength(1024, ErrorMessage = "Length cannot exceed 1024 characters.")]
        public string Comments { get; set; }
    }

    public enum SettingType
    {
        Dashboard
    };
}
