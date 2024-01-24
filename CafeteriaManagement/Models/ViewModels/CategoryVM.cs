using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CafeteriaManagement.Models.ViewModels
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        [ValidateNever]
        public List<Category> Categories { get; set; }
    }
}
