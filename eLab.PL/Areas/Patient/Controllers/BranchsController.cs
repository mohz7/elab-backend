using eLab.BLL.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class BranchsController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchsController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpGet("GetALl")]
        public async Task<IActionResult> GetAllPatient()
        {
            var branchs = await _branchService.GetAllPatientAsync();
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
