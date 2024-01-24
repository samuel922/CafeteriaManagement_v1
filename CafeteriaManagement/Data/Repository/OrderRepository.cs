using CafeteriaManagement.Data.Repository.IRepository;
using CafeteriaManagement.Models;

namespace CafeteriaManagement.Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Order order)
        {
            _db.Update(order);
        }
    }
}
