using OrderService.Models;
using OrderService.Repositories;
using System.Net.Http;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;
using MassTransit;
using MessagingContracts;
using System.ComponentModel.DataAnnotations;
using MassTransit.Transports;
using Newtonsoft.Json;
using OrderService.Controllers;
using System.Net.Http.Headers;


namespace OrderService.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly HttpClient _httpClient;
        private readonly IRequestClient<IStockCheckRequest> _stockCheckClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderService(IOrderRepository orderRepository, HttpClient httpClient, IRequestClient<IStockCheckRequest> stockCheckClient, IPublishEndpoint publishEndpoint)
        {
            _orderRepository = orderRepository;
            _httpClient = httpClient;
            _stockCheckClient = stockCheckClient;
            _publishEndpoint = publishEndpoint;
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


        public async Task AddOrder(Order order)
        {
            await _orderRepository.AddOrder(order);
        }
        public async Task<PlaceOrderResult> PlaceOrderAsync(int productId, int quantity, string userId, string token)
        {
            var response = await _stockCheckClient.GetResponse<IStockCheckResponse>(new
            {
                ProductId = productId,
                Quantity = quantity
            });

            if (!response.Message.IsAvailable)
                return new PlaceOrderResult { Success = false, Message = "Product is out of stock" };

            var orderId = Guid.NewGuid().ToString();

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var productResponse = await httpClient.GetAsync($"http://localhost:5000/products/api/products/{productId}");

            if (!productResponse.IsSuccessStatusCode)
                return new PlaceOrderResult { Success = false, Message = "Failed to fetch product info" };

            var productJson = await productResponse.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(productJson);

            var order = new Order
            {
                Id = orderId,
                ProductId = productId,
                Quantity = quantity,
                UserId = userId,
                TotalPrice = quantity * product.Price
            };

            await _orderRepository.AddOrderAsync(order);

            await _publishEndpoint.Publish(new StockUpdateMessage
            {
                ProductId = productId,
                Quantity = quantity
            });

            await _publishEndpoint.Publish(new OrderNotificationContract
            {
                OrderId = orderId,
                Message = "Your order has been placed successfully!"
            });

            return new PlaceOrderResult { Success = true, OrderId = orderId, Message = "Order placed and notification sent" };
        }
    }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int SellerId { get; set; }
    }
    public class PlaceOrderResult
    {
        public bool Success { get; set; }
        public string OrderId { get; set; }
        public string Message { get; set; }
    }
}
