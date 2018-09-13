using Seed.TriumphSkill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    public interface ISystemSettingService : IService<SystemSetting>
    {
        IQueryable<SystemSetting> All();
        IQueryable<SystemSetting> GetAllByType(SettingType settingType);
        SystemSetting GetSystemSettingById(int id);
        SystemSetting GetSystemSettingByType(SettingType settingType);
        Task AddAsync(SystemSetting systemSetting);
        Task UpdateAsync(SystemSetting systemSetting);
        Task DeleteAsync(int id);
    }
}
