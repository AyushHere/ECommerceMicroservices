using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }  // Foreign Key to Product Service

        [Required]
        public int UserId { get; set; }  // Customer ID

        [Required]
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Shipped, Delivered

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
