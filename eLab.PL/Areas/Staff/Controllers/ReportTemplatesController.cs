using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Models;
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
        
    }
}
