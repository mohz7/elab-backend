using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.DTO.Requests;
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
    public class PatientProfilesController : ControllerBase
    {
        private readonly IPatientProfileService _patientProfileService;

        public PatientProfilesController(IPatientProfileService patientProfileService)
        {
            _patientProfileService = patientProfileService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _patientProfileService.GetAllAsync(staffId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _patientProfileService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RegisterRequest request)
        {
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _patientProfileService.CreateByStaffAsync(request, staffId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdatePatientRequest request)
        {
            var result = await _patientProfileService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string id, [FromBody] ChangePasswordRequest request)
        {
            var result = await _patientProfileService.ChangePasswordAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
