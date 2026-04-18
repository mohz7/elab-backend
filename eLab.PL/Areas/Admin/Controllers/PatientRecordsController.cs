using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLab.PL.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class PatientRecordsController : ControllerBase
    {
        private readonly IPatientRecordService _patientRecordService;

        public PatientRecordsController(IPatientRecordService patientRecordService)
        {
            _patientRecordService = patientRecordService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _patientRecordService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _patientRecordService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("patient/{patientProfileId}")]
        public async Task<IActionResult> GetByPatientProfile(string patientProfileId)
        {
            var result = await _patientRecordService.GetByPatientProfileIdAsync(patientProfileId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PatientRecordRequest request)
        {
            var result = await _patientRecordService.CreateAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _patientRecordService.RemoveAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PatientRecordRequest request)
        {
            var result = await _patientRecordService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}

