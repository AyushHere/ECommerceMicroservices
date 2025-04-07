using OrderService.Models;
using OrderService.Repositories;
using System.Net.Http;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;
using MassTransit;
using MessagingContracts;


namespace OrderService.Services
{
    public class OrderService 
    {
        private readonly IOrderRepository _orderRepository;
        private readonly HttpClient _httpClient;
        private readonly IRequestClient<IStockCheckRequest> _stockCheckClient;

        public OrderService(IOrderRepository orderRepository, HttpClient httpClient, IRequestClient<IStockCheckRequest> stockCheckClient)
        {
            _orderRepository = orderRepository;
            _httpClient = httpClient;
            _stockCheckClient = stockCheckClient;
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
        
    }

    // Represents Product Service response
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
