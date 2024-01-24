using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CafeteriaManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
