using Seed.TriumphSkill.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    internal class Service<T> : IService<T> where T:class
    {
        protected readonly IUnitOfWork unitOfWork;

        public Service(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public PagedListResult<T> Search(SearchQuery<T> searchQuery) 
        {
            return unitOfWork.Repository<T>().Search(searchQuery);
        }
    }
}
