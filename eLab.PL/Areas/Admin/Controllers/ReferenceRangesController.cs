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
    public class ReferenceRangesController : ControllerBase
    {
        private readonly IReferenceRangeService _referenceRangeService;

        public ReferenceRangesController(IReferenceRangeService referenceRangeService)
        {
            _referenceRangeService = referenceRangeService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? ReportTemplateId)
        {
            var result = await _referenceRangeService.GetAllAsync(ReportTemplateId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _referenceRangeService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ReferenceRangeRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _referenceRangeService.CreateAsync(request, adminId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var result = await _referenceRangeService.RemoveAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ReferenceRangeUpdateRequest request)
        {
            var result = await _referenceRangeService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
