using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class AIsController : ControllerBase
    {
        private readonly IAIChatService _aiChatService;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public AIsController(IAIChatService aiChatService) => _aiChatService = aiChatService;

        [HttpPost("sessions")]
        public async Task<IActionResult> StartSession([FromBody] StartAISessionRequest request)
        {
            var result = await _aiChatService.StartSessionAsync(request.ResultId, UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetMySessions()
        {
            var result = await _aiChatService.GetMySessionsAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("sessions/{id}")]
        public async Task<IActionResult> GetSession(int id)
        {
            var result = await _aiChatService.GetSessionAsync(id, UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("sessions/{id}/messages")]
        public async Task<IActionResult> SendMessage(int id, [FromBody] AIMessageRequest request)
        {
            var result = await _aiChatService.SendMessageAsync(id, UserId, request.Message);
            return StatusCode(result.StatusCode, result);
        }
    }
}
