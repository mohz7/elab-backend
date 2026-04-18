using eLab.BLL.Services.Interface;
using eLab.DAL.Models;
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
            var result = await _branchService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _branchService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
