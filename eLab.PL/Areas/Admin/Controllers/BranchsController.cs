using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
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
            var branchs = await _branchService.GetAllAsync();
            return Ok(branchs);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _branchService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]BranchRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _branchService.CreateAsync(request,userId);
            return Ok(result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BranchRequest request)
        {
            var result = await _branchService.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Remove([FromHeader] int id)
        {
            var result = await _branchService.RemoveAsync(id);
            return Ok(result);
        }
    }
}
