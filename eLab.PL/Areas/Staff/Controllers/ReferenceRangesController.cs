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
        
    }
}
