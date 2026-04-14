using eLab.BLL.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLab.PL.Areas.Staff.Controllers
{
    [Route("api/[Area]/[controller]")]
    [ApiController]
    [Area("staff")]
    [Authorize(Roles = "Staff")]
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
    }
}
