using CafeteriaManagement.Models;

namespace CafeteriaManagement.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
