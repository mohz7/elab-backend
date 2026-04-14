using eLab.BLL.Services.Interface;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLab.PL.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetBookingByStatus(Status status)
        {
            var bookings = await _bookingService.GetByStatusAsync(status);
            return Ok(bookings);
        }
        [HttpPatch("change-status/{orderId}")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, [FromBody] Status newStatus)
        {
            var result = await _bookingService.ChangeStatusAsync(orderId, newStatus);
            return Ok(new { message = "status is changed" });
        }
    }
}
