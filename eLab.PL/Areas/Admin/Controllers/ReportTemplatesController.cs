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
    public class ReportTemplatesController : ControllerBase
    {
        private readonly IReportTemplateService _reportTemplateService;

        public ReportTemplatesController(IReportTemplateService reportTemplateService)
        {
            _reportTemplateService = reportTemplateService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? testCatalogId)
        {
            var result = await _reportTemplateService.GetAllAsync(testCatalogId);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _reportTemplateService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ReportTemplateRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reportTemplateService.CreateAsync(request, adminId);
            return Ok(result);
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var result = await _reportTemplateService.RemoveAsync(id);
            return Ok(result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromHeader] int id, [FromBody] ReportTemplateRequest request)
        {
            var result = await _reportTemplateService.UpdateAsync(id, request);
            return Ok(request);
        }
    }
}
