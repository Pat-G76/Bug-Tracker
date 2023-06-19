using Bug_Tracker.Data.DataAccess;
using System.Linq.Expressions;

namespace Bug_Tracker.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        protected AppDbContext dbContext;

        public RepositoryBase(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void Add(T item)
        {
            dbContext.Set<T>().Add(item);
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return dbContext.Set<T>().Where(expression);
        }

        public IEnumerable<T> GetAllItems()
        {
            return dbContext.Set<T>();
        }

        public T GetById(int id)
        {
            return dbContext.Set<T>().Find(id);
        }

        public void Remove(T item)
        {
            dbContext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            dbContext.Set<T>().Update(item);
        }

		public IEnumerable<T> GetWithOptions(QueryOptions<T> options)
        {
            IQueryable<T> items = dbContext.Set<T>();

            if(options.HasWhere)
            {
                items = items.Where(options.Where);
            }
            
            if(options.HasOrderBy)
            {
                if (options.OrderByDirection == "asc")
                {
                    items = items.OrderBy(options.OrderBy);
                }
                else
                {
					items = items.OrderByDescending(options.OrderBy);
				}
            }

            if(options.HasPaging)
            {
                items = items.Skip( (options.PageNumber - 1) * options.PageSize).Take(options.PageSize);
            }


            return items;

        }


	}
}
