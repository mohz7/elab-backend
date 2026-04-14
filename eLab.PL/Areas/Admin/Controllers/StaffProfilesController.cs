using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var staffs = await _staffProfilesService.GetAllAsync(branchId, job, IsActive);
            return Ok(staffs);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _staffProfilesService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StaffProfileRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _staffProfilesService.CreateAsync(request, adminId);
            return Ok(result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] StaffProfileRequest request)
        {
            var result = await _staffProfilesService.UpdateAsync(id, request);
            return Ok(result);
        }
        [HttpDelete("InActive/{id}")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var result = await _staffProfilesService.RemoveAsync(id);
            return Ok(result);
        }
    }
}
