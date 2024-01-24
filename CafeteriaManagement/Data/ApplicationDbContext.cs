using CafeteriaManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CafeteriaManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Brake Fast", DisplayOrder = 1},
                new Category { Id = 2, Name = "Lunch", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Dinner", DisplayOrder = 3 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Chapati",
                    Price = 20,
                    ImageUrl = "",
                    CategoryId = 1
                },
                 new Product
                 {
                     Id = 2,
                     Name = "Gidheri",
                     Price = 40,
                     ImageUrl = "",
                     CategoryId = 2
                 },
                  new Product
                  {
                      Id = 3,
                      Name = "Pilau",
                      Price = 70,
                      ImageUrl = "",
                      CategoryId = 2
                  },
                   new Product
                   {
                       Id = 4,
                       Name = "Stew",
                       Price = 120,
                       ImageUrl = "",
                       CategoryId = 2
                   }
            ) ;
        }
    }
}
