using eLab.BLL.Services.Classes;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Staff.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("staff")]
    [Authorize(Roles = "Staff")]
    public class StaffProfilesController : ControllerBase
    {
        private readonly IStaffProfileService _staffProfilesService;

        public StaffProfilesController(IStaffProfileService staffProfilesService)
        {
            _staffProfilesService = staffProfilesService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? branchId, [FromQuery] JobTitle? job, [FromQuery] bool? IsActive)
        {
            var result = await _staffProfilesService.GetAllAsync(branchId, job, IsActive);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _staffProfilesService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _staffProfilesService.ChangePasswordAsync(userId, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
