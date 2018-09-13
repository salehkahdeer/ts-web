using Seed.TriumphSkill.Models;
using Seed.TriumphSkill.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class VillageService : Service<Village>, IVillageService
    {
        private readonly IRepository<Village> villageRepository;

        public VillageService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.villageRepository = unitOfWork.Repository<Village>();
        }

        public IQueryable<Village> All()
        {
            return villageRepository.All();
        }

        public Village GetVillageById(int id)
        {
            return villageRepository.Find(c => c.ID == id).FirstOrDefault();
        }

        public async Task AddAsync(Village village)
        {
            this.villageRepository.Add(village);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Village village)
        {
            this.villageRepository.Update(village);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var village = this.villageRepository.Find(c => c.ID == id).FirstOrDefault();
            this.villageRepository.Remove(village);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
