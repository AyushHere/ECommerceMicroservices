namespace NotificationService.Contracts
{
    public class OrderStatusChanged
    {
        public string OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
