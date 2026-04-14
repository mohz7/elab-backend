using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.AddToCartAsync(request, userId);
            return Ok(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetUserCart()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.CartSummaryResponesAsync(UserId);
            return Ok(result);
        }
    }
}
