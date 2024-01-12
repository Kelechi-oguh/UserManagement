using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.ProductService;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            return Ok(await  _productService.GetProduct(productId));
        }

        [HttpGet("all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }
    }
}
