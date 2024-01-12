using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.CartService;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllCarts()
        {
            return Ok(await _cartService.GetAllCarts());
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartById(int cartId)
        {
            return Ok(await _cartService.GetCartById(cartId));
        }
    }
}
