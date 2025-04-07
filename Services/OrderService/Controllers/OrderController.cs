using MassTransit;
using MessagingContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderService.Models;
using OrderService.Services;
using System.Net.Http.Headers;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService.Services.OrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<IStockCheckRequest> _stockClient;

        public OrderController(OrderService.Services.OrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return Ok(await _orderService.GetAllOrders());
        }
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Seller")]
        [Authorize(Policy = "Customer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [Authorize(Policy = "Customer")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(string id, Order order)
        {
            if (id != order.Id) return BadRequest();
            await _orderService.UpdateOrder(order);
            return NoContent();
        }

        [Authorize(Policy = "Customer")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return NoContent();
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromQuery] int id, [FromQuery] int quantity)
        {
            var stockResponse = await _stockClient.GetResponse<IStockCheckResponse>(new
            {
                ProductId = id,
                Quantity = quantity
            });

            if (!stockResponse.Message.IsAvailable)
            {
                return BadRequest("Product is out of stock");
            }

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = User.FindFirst("UserId")?.Value;

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"http://localhost:5000/products/api/products/{id}";

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to fetch product info");
            }

            var productJson = await response.Content.ReadAsStringAsync();
            var productDetails = JsonConvert.DeserializeObject<Product>(productJson);

            var orderId = Guid.NewGuid().ToString();
            var addOrder = new Order
            {
                Id = orderId,
                ProductId = id,
                Quantity = quantity,
                UserId = userId,
                TotalPrice = quantity * productDetails.Price,
            };

            await _orderService.AddOrder(addOrder);

            await _publishEndpoint.Publish(new StockUpdateMessage
            {
                ProductId = id,
                Quantity = quantity
            });

            var notification = new OrderNotificationContract
            {
                OrderId = orderId,
                Message = "Your order has been placed successfully!"
            };
            await _publishEndpoint.Publish(notification);
            return Ok(new { OrderId = orderId, Status = "Order placed and notification sent" });
        }
    }
}