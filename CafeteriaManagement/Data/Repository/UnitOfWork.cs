using CafeteriaManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaManagement.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IOrderRepository Order { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Order = new OrderRepository(_db);
        }
        public void Save()
        {
            try
            {
                // Perform EF Core operations here
                _db.SaveChanges();
            }
            catch (InvalidOperationException ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine("InvalidOperationException: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Log or handle other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            
        }
    }
}
