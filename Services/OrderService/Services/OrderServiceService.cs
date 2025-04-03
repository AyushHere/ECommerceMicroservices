using OrderService.Models;
using OrderService.Repositories;
using OrderService.Intercomms;
using System.Net.Http;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;


namespace OrderService.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly HttpClient _httpClient;

        public OrderService(IOrderRepository orderRepository, HttpClient httpClient)
        {
            _orderRepository = orderRepository;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _orderRepository.GetOrderById(id);
        }

   

        public async Task UpdateOrder(Order order)
        {
            await _orderRepository.UpdateOrder(order);
        }

        public async Task DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrder(id);
        }


        public async Task<bool> PlaceOrder(Order order)
        {
            // Save Order
            await _orderRepository.AddOrder(order);

            // Publish Notification to RabbitMQ
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "OrderNotifications", durable: false, exclusive: false, autoDelete: false, arguments: null);

            string message = $"Order {order.Id} placed successfully!";
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: "OrderNotifications", basicProperties: null, body: body);

            return true;
        }
    }

    // Represents Product Service response
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
