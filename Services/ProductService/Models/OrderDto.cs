namespace ProductService.Models
{
    public class OrderDto
    {
        public int OrderId { get; set; }  // Optional if not required for stock update
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
