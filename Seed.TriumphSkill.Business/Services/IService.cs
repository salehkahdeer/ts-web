using Seed.TriumphSkill.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.TriumphSkill.Services
{
    public interface IService<T>
    {
        PagedListResult<T> Search(SearchQuery<T> searchQuery);
    }
}
