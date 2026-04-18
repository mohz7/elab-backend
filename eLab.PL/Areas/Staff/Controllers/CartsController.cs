using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Staff.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("staff")]
    [Authorize(Roles = "Staff")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddToCart([FromRoute] string userId,[FromBody] CartRequest request)
        {
            var result = await _cartService.AddToCartAsync(request, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserCart([FromRoute] string userId)
        {
            var result = await _cartService.CartSummaryResponesAsync(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
