namespace CafeteriaManagement.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }

        public IOrderRepository Order { get; }
        void Save();
    }
}
