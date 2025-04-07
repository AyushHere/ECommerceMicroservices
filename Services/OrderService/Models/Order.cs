using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {

        public string Id { get; set; }

        public int ProductId { get; set; }


        public string UserId { get; set; } 

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
