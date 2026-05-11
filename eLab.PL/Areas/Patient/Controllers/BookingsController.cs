using eLab.BLL.Services.Interface;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetAllForPatient")]
        public async Task<IActionResult> GetAllForPatient()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _bookingService.GetBookingByPatientAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetByBookingId/{bookingId}")]
        public async Task<IActionResult> GetByBookingId([FromRoute] int bookingId)
        {
            var result = await _bookingService.GetByIdAsync(bookingId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
