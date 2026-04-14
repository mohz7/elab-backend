using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
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
    public class PricesController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PricesController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet("GetALl")]
        public async Task<IActionResult> GetAll([FromQuery] int? branchId)
        {
            var branchs = await _priceService.GetAllAsync(branchId);
            return Ok(branchs);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _priceService.GetByIdAsync(id);
            return Ok(result);
        }
        
    }
}
