using eLab.BLL.Services.Classes;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
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
    public class TestCatalogsController : ControllerBase
    {
        private readonly ITestCatalogService _testCatalogService;

        public TestCatalogsController(ITestCatalogService testCatalogService)
        {
            _testCatalogService = testCatalogService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _testCatalogService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var result = await _testCatalogService.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] TestCatalogRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _testCatalogService.CreateAsync(request, adminId);
            return Ok(result);
        }
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Upadte([FromRoute] int id,[FromBody] TestCatalogRequest request)
        {
            var result = await _testCatalogService.UpdateAsync(id, request);
            return Ok(result);
        }
        [HttpDelete("Deactivate/{id}")]
        public async Task<IActionResult> DeactivateTestCatalog([FromRoute] int id)
        {
            var result = await _testCatalogService.DeactivateTestCatalogAsync(id);
            return Ok(result);
        }
        [HttpPatch("Activate/{id}")]
        public async Task<IActionResult> ActivateTestCatalog([FromRoute] int id)
        {
            var result = await _testCatalogService.ActivateTestCatalogAsync(id);
            return Ok(result);
        }
    }
}
