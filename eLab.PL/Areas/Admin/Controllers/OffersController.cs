using eLab.BLL.Services.Classes;
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
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] int? branchId)
        {
            var result = await _offerService.GetAllAsync(branchId, false);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _offerService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OfferRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _offerService.CreateAsync(request, userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OfferRequest request)
        {
            var result = await _offerService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Deactivate/{id}")]
        public async Task<IActionResult> DeactivateTestCatalog([FromRoute] int id)
        {
            var result = await _offerService.DeactivateTestCatalogAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Activate/{id}")]
        public async Task<IActionResult> ActivateTestCatalog([FromRoute] int id)
        {
            var result = await _offerService.ActivateTestCatalogAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
