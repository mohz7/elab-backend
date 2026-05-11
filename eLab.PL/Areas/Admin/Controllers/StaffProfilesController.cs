using eLab.BLL.Services.Classes;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("admin")]
    [Authorize(Roles = "Admin")]
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
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StaffProfileRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _staffProfilesService.CreateAsync(request, adminId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateStaffProfileRequest request)
        {
            var result = await _staffProfilesService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("InActive/{id}")]
        public async Task<IActionResult> InActive([FromRoute] string id)
        {
            var result = await _staffProfilesService.InActiveAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("Active/{id}")]
        public async Task<IActionResult> Active([FromRoute] string id)
        {
            var result = await _staffProfilesService.ActiveAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string id, [FromBody] ChangePasswordRequest request)
        {
            var result = await _staffProfilesService.ChangePasswordAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
