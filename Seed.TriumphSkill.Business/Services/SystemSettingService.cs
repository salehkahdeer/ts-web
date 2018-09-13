using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class SystemSettingService : Service<SystemSetting>, ISystemSettingService
    {
        private readonly IRepository<SystemSetting> systemSettingRepository;

        public SystemSettingService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.systemSettingRepository = unitOfWork.Repository<SystemSetting>();
        }

        public IQueryable<SystemSetting> All()
        {
            return systemSettingRepository.All();
        }

        public IQueryable<SystemSetting> GetAllByType(SettingType settingType)
        {
            string settingTypeValue = settingType.ToString();
            return systemSettingRepository.Find(c => c.SettingTypeValue == settingTypeValue);
        }

        public SystemSetting GetSystemSettingById(int id)
        {
            return systemSettingRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public SystemSetting GetSystemSettingByType(SettingType settingType)
        {
            string settingTypeValue = settingType.ToString();
            return systemSettingRepository.Find(c => c.SettingTypeValue == settingTypeValue).FirstOrDefault();
        }

        public async Task AddAsync(SystemSetting systemSetting)
        {
            this.systemSettingRepository.Add(systemSetting);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(SystemSetting systemSetting)
        {
            this.systemSettingRepository.Update(systemSetting);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var systemSetting = this.systemSettingRepository.Find(c => c.ID == id).FirstOrDefault();
            this.systemSettingRepository.Remove(systemSetting);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
