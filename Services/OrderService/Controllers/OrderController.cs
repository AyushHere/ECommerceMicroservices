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
  

        public OrderController(OrderService.Services.OrderService orderService)
        {
            _orderService = orderService;
           
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

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = User.FindFirst("UserId")?.Value;

            var result = await _orderService.PlaceOrderAsync(id, quantity, userId, token);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { result.OrderId, result.Message });


        }
    }
}