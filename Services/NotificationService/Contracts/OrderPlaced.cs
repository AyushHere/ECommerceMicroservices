namespace NotificationService.Contracts
{
    public class OrderPlaced
    {
        public string OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
