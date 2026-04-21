using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.DTO.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class PatientProfilesController : ControllerBase
    {
        private readonly IPatientProfileService _patientProfileService;

        public PatientProfilesController(IPatientProfileService patientProfileService)
        {
            _patientProfileService = patientProfileService;
        }

        [HttpGet("GetMyProfile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _patientProfileService.GetByPatientAsync(patientId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RegisterRequest request)
        {
            var result = await _patientProfileService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _patientProfileService.ChangePasswordAsync(patientId, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
