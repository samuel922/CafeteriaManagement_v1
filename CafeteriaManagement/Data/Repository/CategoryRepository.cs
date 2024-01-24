using CafeteriaManagement.Data.Repository.IRepository;
using CafeteriaManagement.Models;

namespace CafeteriaManagement.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category category)
        {
            _db.Update(category);
        }
    }
}
