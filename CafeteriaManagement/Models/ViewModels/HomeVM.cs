namespace CafeteriaManagement.Models.ViewModels
{
    public class HomeVM
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
