using Bug_Tracker.Data.DataAccess;
using System.Linq.Expressions;

namespace Bug_Tracker.Data
{
    public interface IRepositoryBase<T>
    {
        T GetById(int id);
        void Add(T item);
        void Remove(T item);
        void Update(T item);
        IEnumerable<T> GetAllItems();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetWithOptions(QueryOptions<T> options);
    }
}
