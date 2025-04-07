using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProductService.Repositories;
using ProductService.Models;
using Microsoft.AspNetCore.Authorization;
namespace UserService.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService.Service.ProductService _productService;

        public ProductController(ProductService.Service.ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Seller")]
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            await _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Seller")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            await _productService.UpdateProduct(product);
            return NoContent();
        }
        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Seller")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
