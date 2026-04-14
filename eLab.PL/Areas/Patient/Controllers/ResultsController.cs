using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        public ResultController(IResultService resultService)
            => _resultService = resultService;

        // Anyone sees a single result (access checked in service)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _resultService.GetByIdAsync(id, UserId);
            return Ok(result);
        }

        // Patient gets their full result history
        [HttpGet("my")]
        public async Task<IActionResult> GetMyResults()
        {
            // get patientProfileId from claims or look it up
            var patientProfileId = User.FindFirstValue("PatientProfileId")!;
            var results = await _resultService.GetMyResultsAsync(patientProfileId);
            return Ok(results);
        }
    }
}
