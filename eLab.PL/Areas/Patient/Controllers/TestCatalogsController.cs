using eLab.BLL.Services.Classes;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
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
            var result = await _testCatalogService.GetAllAsync(true);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromQuery]int id)
        {
            var result = await _testCatalogService.GetByIdAsync(id);
            return Ok(result);
        }
    }
}
