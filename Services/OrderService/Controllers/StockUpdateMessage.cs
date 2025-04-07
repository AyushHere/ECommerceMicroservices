
namespace OrderService.Controllers
{
    public class StockUpdateMessage { 
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}