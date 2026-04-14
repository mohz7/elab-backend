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
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        public ResultController(IResultService resultService)
            => _resultService = resultService;

        // Staff uploads a new result
        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] UploadResultRequest request)
        {
            var result = await _resultService
                .UploadResultAsync(request, UserId);

            return Ok(result);
        }

        // Anyone sees a single result (access checked in service)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _resultService
                .GetByIdAsync(id, UserId);

            return Ok(result);
        }

        // Staff sees results waiting for their approval
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var results = await _resultService
                .GetPendingApprovalAsync(UserId);

            return Ok(results);
        }

        // Staff approves or rejects a result
        [HttpPatch("{id}/review")]
        public async Task<IActionResult> Review(
            int id, [FromBody] ReviewResultRequest request)
        {
            var result = await _resultService
                .ReviewResultAsync(id, request, UserId);

            return Ok(result);
        }
    }
}
