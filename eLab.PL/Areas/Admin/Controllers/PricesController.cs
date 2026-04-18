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
    public class PricesController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PricesController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet("getALl")]
        public async Task<IActionResult> GetAll([FromQuery] int? branchId)
        {
            var result = await _priceService.GetAllAsync(branchId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _priceService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PriceRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _priceService.CreateAsync(request, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PriceRequest request)
        {
            var result = await _priceService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var result = await _priceService.RemoveAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
