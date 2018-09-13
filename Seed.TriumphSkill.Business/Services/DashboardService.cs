using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class DashboardService : Service<Dashboard>, IDashboardService
    {
        private readonly IRepository<Dashboard> dashboardRepository;

        public DashboardService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.dashboardRepository = unitOfWork.Repository<Dashboard>();
        }

        public IQueryable<Dashboard> All()
        {
            return dashboardRepository.All();
        }

        public Dashboard GetDashboardById(int id)
        {
            return dashboardRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public Dashboard GetDashboardByUser(int userId, string name)
        {
            return dashboardRepository.Find(c => c.User_ID == userId && c.Name == name).FirstOrDefault();
        }

        public async Task AddAsync(Dashboard dashboard)
        {
            this.dashboardRepository.Add(dashboard);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dashboard dashboard)
        {
            this.dashboardRepository.Update(dashboard);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dashboard = this.dashboardRepository.Find(c => c.ID == id).FirstOrDefault();
            this.dashboardRepository.Remove(dashboard);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
