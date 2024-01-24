using CafeteriaManagement.Models;

namespace CafeteriaManagement.Data.Repository.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
    }
}
