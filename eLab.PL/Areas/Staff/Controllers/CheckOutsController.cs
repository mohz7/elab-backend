using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Staff.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("staff")]
    [Authorize(Roles = "Staff")]
    public class CheckOutsController : ControllerBase
    {
        private readonly ICheckOutService _checkOutService;

        public CheckOutsController(ICheckOutService checkOutService)
        {
            _checkOutService = checkOutService;
        }
        [HttpPost("payment/{patientId}")]
        public async Task<IActionResult> Payment([FromRoute] string patientId, [FromBody] CheckOutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _checkOutService.ProcessPaymentByStaffAsync(patientId, request, userId, Request);
            return StatusCode(result.StatusCode, result);
        }

    }
}
