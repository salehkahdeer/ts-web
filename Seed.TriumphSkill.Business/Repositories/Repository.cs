using Autofac;
using Seed.TriumphSkill.Contexts;
using Seed.TriumphSkill.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Seed.TriumphSkill.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        internal DbContext _context
        {
            get;
            set;
        }

        protected IDbSet<T> DbSet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        private bool _disposed;
        public Repository(DbContext context)
        {
            _context = context;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();

            _disposed = true;
        }

        public virtual void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            this._context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public T FindSingle(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var set = FindIncluding(includes);
            return (predicate == null) ?
                   set.FirstOrDefault() :
                   set.FirstOrDefault(predicate);
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var set = addIncludeProperties(includes);
            return (predicate == null) ?
                   await set.SingleOrDefaultAsync() :
                   await set.SingleOrDefaultAsync(predicate);
        }

        public IQueryable<T> All()
        {
            return DbSet.AsQueryable<T>();
        }

        public async Task<List<T>> AllAsync()
        {
            return await DbSet.AsQueryable().ToListAsync<T>();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var set = FindIncluding(includes);
            return (predicate == null) ? set : set.Where(predicate);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var set = addIncludeProperties(includes);

            return (predicate == null) ? await set.ToListAsync() : await set.Where(predicate).ToListAsync();
        }

        public IQueryable<T> FindIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            var set = addIncludeProperties(includeProperties);
            return set.AsQueryable();
        }

        public async Task<List<T>> FindIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            var set = addIncludeProperties(includeProperties);
            return await set.ToListAsync();
        }

        private IDbSet<T> addIncludeProperties(params Expression<Func<T, object>>[] includeProperties)
        {
            var set = this._context.Set<T>();

            if (includeProperties != null)
            {
                foreach (var include in includeProperties)
                {
                    set.Include(include);
                }
            }
            return set;
        }

        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<T>();
        }

        public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync<T>();
        }

        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> filter, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            return await _resetSet.ToListAsync();
        }

        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            var set = this._context.Set<T>();
            return (predicate == null) ?
                   set.Count() :
                   set.Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            var set = this._context.Set<T>();
            return (predicate == null) ?
                            await set.CountAsync() :
                            await set.CountAsync(predicate);
        }

        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate) > 0;
        }

        public async Task<bool> ContainsAsync(Expression<Func<T, bool>> predicate)
        {
            return await CountAsync(predicate) > 0;
        }

        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            var set = this._context.Set<T>();
            return (predicate == null) ? set.Any() : set.Any(predicate);
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate = null)
        {
            var set = this._context.Set<T>();
            return (predicate == null) ? await set.AnyAsync() : await set.AnyAsync(predicate);
        }

        public virtual void Remove(T entity)
        {
            this._context.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
        }

        public PagedListResult<T> Search(SearchQuery<T> searchQuery)
        {
            IQueryable<T> sequence = DbSet;

            //Applying filters
            sequence = ManageFilters(searchQuery, sequence);

            //Include Properties
            sequence = ManageIncludeProperties(searchQuery, sequence);

            //Resolving Sort Criteria
            //This code applies the sorting criterias sent as the parameter
            sequence = ManageSortCriterias(searchQuery, sequence);

            return GetTheResult(searchQuery, sequence); ;
        }

        public PagedListResult<T> Search(SearchQuery<T> searchQuery, out string query)
        {
            IQueryable<T> sequence = DbSet;

            //Applying filters
            sequence = ManageFilters(searchQuery, sequence);

            //Include Properties
            sequence = ManageIncludeProperties(searchQuery, sequence);

            //Resolving Sort Criteria
            //This code applies the sorting criterias sent as the parameter
            sequence = ManageSortCriterias(searchQuery, sequence);

            return GetTheResult(searchQuery, sequence, out query); ;
        }

        /// <summary>
        /// Executes the query against the repository (database).
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        protected virtual PagedListResult<T> GetTheResult(SearchQuery<T> searchQuery, IQueryable<T> sequence)
        {
            //Counting the total number of object.
            var resultCount = sequence.Count();

            var result = (searchQuery.Take > 0)
                                ? (sequence.Skip(searchQuery.Skip).Take(searchQuery.Take).ToList())
                                : (sequence.ToList());

            //Debug info of what the query looks like
            //System.Diagnostics.Debug.WriteLine(sequence.ToString());

            // Setting up the return object.
            bool hasNext = (searchQuery.Skip <= 0 && searchQuery.Take <= 0) ? false : (searchQuery.Skip + searchQuery.Take < resultCount);
            return new PagedListResult<T>()
            {
                Entities = result,
                HasNext = hasNext,
                HasPrevious = (searchQuery.Skip > 0),
                Count = resultCount
            };
        }

        /// <summary>
        /// Executes the query against the repository (database).
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        protected virtual PagedListResult<T> GetTheResult(SearchQuery<T> searchQuery, IQueryable<T> sequence, out string query)
        {
            query = sequence.ToString();

            //Counting the total number of object.
            var resultCount = sequence.Count();


            var result = (searchQuery.Take > 0)
                                ? (sequence.Skip(searchQuery.Skip).Take(searchQuery.Take).ToList())
                                : (sequence.ToList());

            //Debug info of what the query looks like
            //System.Diagnostics.Debug.WriteLine(sequence.ToString());

            // Setting up the return object.
            bool hasNext = (searchQuery.Skip <= 0 && searchQuery.Take <= 0) ? false : (searchQuery.Skip + searchQuery.Take < resultCount);
            return new PagedListResult<T>()
            {
                Entities = result,
                HasNext = hasNext,
                HasPrevious = (searchQuery.Skip > 0),
                Count = resultCount
            };
        }

        /// <summary>
        /// Resolves and applies the sorting criteria of the SearchQuery
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        protected virtual IQueryable<T> ManageSortCriterias(SearchQuery<T> searchQuery, IQueryable<T> sequence)
        {
            if (searchQuery.SortCriterias != null && searchQuery.SortCriterias.Count > 0)
            {
                var sortCriteria = searchQuery.SortCriterias[0];
                var orderedSequence = sortCriteria.ApplyOrdering(sequence, false);

                if (searchQuery.SortCriterias.Count > 1)
                {
                    for (var i = 1; i < searchQuery.SortCriterias.Count; i++)
                    {
                        var sc = searchQuery.SortCriterias[i];
                        orderedSequence = sc.ApplyOrdering(orderedSequence, true);
                    }
                }
                sequence = orderedSequence;
            }
            else
            {
                sequence = ((IOrderedQueryable<T>)sequence).OrderBy(x => (true));
            }
            return sequence;
        }

        /// <summary>
        /// Chains the where clause to the IQueriable instance.
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        protected virtual IQueryable<T> ManageFilters(SearchQuery<T> searchQuery, IQueryable<T> sequence)
        {
            if (searchQuery.Filters != null && searchQuery.Filters.Count > 0)
            {
                foreach (var filterClause in searchQuery.Filters)
                {
                    sequence = sequence.Where(filterClause);
                }
            }
            return sequence;
        }

        /// <summary>
        /// Includes the properties sent as part of the SearchQuery.
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        protected virtual IQueryable<T> ManageIncludeProperties(SearchQuery<T> searchQuery, IQueryable<T> sequence)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery.IncludeProperties))
            {
                var properties = searchQuery.IncludeProperties.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var includeProperty in properties)
                {
                    sequence = sequence.Include(includeProperty);
                }
            }
            return sequence;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
