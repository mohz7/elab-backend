using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eLab.PL.Areas.Patient.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("patient")]
    [Authorize(Roles = "Patient")]
    public class ChatsController : ControllerBase
    {
        private readonly IStaffChatService _chatService;
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        public ChatsController(IStaffChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> CreateSession([FromBody] StaffChatRequest request)
        {
            var result = await _chatService.CreateSessionAsync(request.ResultId, UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> GetMySessions()
        {
            var result = await _chatService.GetByPatientIdAsync(UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("sessions/{id}")]
        public async Task<IActionResult> GetSession(int id)
        {
            var result = await _chatService.GetSessionAsync(id, UserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("sessions/{id}/messages")]
        public async Task<IActionResult> SendMessage(int id, [FromBody] StaffChatMessageRequest dto)
        {
            var result = await _chatService.SendMessageAsync(id, UserId, dto.Message);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("sessions/{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var result = await _chatService.MarkAsReadAsync(id, UserId);
            return StatusCode(result.StatusCode, result);
        }
    }
}