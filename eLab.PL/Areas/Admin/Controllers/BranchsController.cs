using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Admin.Controllers
{
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class BranchsController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchsController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpGet("GetALl")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _branchService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _branchService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]BranchRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _branchService.CreateAsync(request,userId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BranchRequest request)
        {
            var result = await _branchService.UpdateAsync(id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("Deactivate/{id}")]
        public async Task<IActionResult> Deactivate([FromRoute] int id)
        {
            var result = await _branchService.DeactivateAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("Activate/{id}")]
        public async Task<IActionResult> Activate([FromRoute] int id)
        {
            var result = await _branchService.ActivateAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
