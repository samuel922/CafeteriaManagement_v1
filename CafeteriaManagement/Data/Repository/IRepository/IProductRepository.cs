using CafeteriaManagement.Models;

namespace CafeteriaManagement.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
