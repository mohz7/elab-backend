using eLab.BLL.Services.Classes;
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
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet("GetALl")]
        public async Task<IActionResult> GetAll([FromQuery] int? branchId)
        {
            var branchs = await _offerService.GetAllAsync(branchId, false);
            return Ok(branchs);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _offerService.GetByIdAsync(id);
            return Ok(result);
        }
        
    }
}
