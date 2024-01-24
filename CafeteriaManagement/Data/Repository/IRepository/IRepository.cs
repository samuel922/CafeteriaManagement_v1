using System.Linq.Expressions;

namespace CafeteriaManagement.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //Get all resources
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null);
        //Get a single resource
        T Get(Expression<Func<T, bool>> filter);

        //Add
        void Add(T entity);
        //Remove
        void Remove(T entity);
        //Remove Range
        void RemoveRange(IEnumerable<T> entities);
    }
}
