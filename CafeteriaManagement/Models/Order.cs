using CafeteriaManagement.Utilities;
using System.ComponentModel.DataAnnotations;

namespace CafeteriaManagement.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string OrderNumber { get; set; }

        // Navigation property representing the associated products
        public List<Product> Products { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }
        public decimal TotalPrice { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public decimal Balance { get; set; }

        public DateTime OrderTime { get; set; }

        public Order()
        {
            OrderNumber = GenerateOrderNo.GenerateOrderNumber();
        }
    }

}
