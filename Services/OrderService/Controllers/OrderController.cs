using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService.Services.OrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        //[HttpPost]
        //public async Task<ActionResult> PlaceOrder(Order order)
        //{
        //    var success = await _orderService.PlaceOrder(order);
        //    if (!success)
        //    {
        //        return BadRequest("Order could not be placed. Product may be out of stock.");
        //    }
        //    return Ok("Order placed successfully.");
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id) return BadRequest();
            await _orderService.UpdateOrder(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return NoContent();
        }
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder()
        {
            // Your order processing logic here...
            var orderId = Guid.NewGuid().ToString();

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