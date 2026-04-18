using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class CheckOutsController : ControllerBase
    {
        private readonly ICheckOutService _checkOutService;

        public CheckOutsController(ICheckOutService checkOutService)
        {
            _checkOutService = checkOutService;
        }
        [HttpPost("payment")]
        public async Task<IActionResult> Payment(CheckOutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _checkOutService.ProcessPaymentAsync(request, userId, Request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("success/{bookingId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromRoute] int bookingId)
        {
            var result = await _checkOutService.HandlePaymentSuccessAsync(bookingId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
