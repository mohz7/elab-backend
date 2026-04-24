using eLab.BLL.Services.Interface;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Midicare_eLab.DAL.Models;

namespace eLab.PL.Areas.Staff.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("staff")]
    [Authorize(Roles = "Staff")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetAll/{branchId}")]
        public async Task<IActionResult> GetAll([FromQuery] int? branchId)
        {
            var result = await _bookingService.GetAll(branchId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetAllForPatient/{patientId}")]
        public async Task<IActionResult> GetAllForPatient([FromRoute] string patientId)
        {
            var result = await _bookingService.GetBookingByPatientAsync(patientId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetPatientByBookingId/{bookingId}")]
        public async Task<IActionResult> GetPatientByBooking([FromRoute] int bookingId)
        {
            var result = await _bookingService.GetUserByBookingAsync(bookingId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetBookingByStatus(Status status)
        {
            var result = await _bookingService.GetByStatusAsync(status);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("change-status/{orderId}")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, [FromBody] Status newStatus)
        {
            var result = await _bookingService.ChangeStatusAsync(orderId, newStatus);
            return Ok(new { message = "status is changed" });
        }
    }
}
