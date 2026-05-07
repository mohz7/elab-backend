using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? branchId)
        {
            var result = await _bookingService.GetAll(branchId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetByBookingId/{bookingId}")]
        public async Task<IActionResult> GetByBookingId([FromRoute] int bookingId)
        {
            var result = await _bookingService.GetByIdAsync(bookingId);
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

        [HttpPatch("change-status/{bookingId}")]
        public async Task<IActionResult> ChangeOrderStatus(int bookingId, [FromBody] Change_statusRequest newStatus)
        {
            var result = await _bookingService.ChangeStatusAsync(bookingId, newStatus);
            if (!result.Success)
                return BadRequest(new { message = "Failed to change status" });
            return Ok(new { message = "status is changed" });
        }
    }
}
