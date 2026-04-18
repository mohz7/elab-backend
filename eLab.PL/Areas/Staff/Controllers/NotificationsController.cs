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
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.GetAllByUserAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetById([FromRoute] string userId)
        {
            var result = await _notificationService.GetAllByUserAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.GetByIdAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { unreadCount = count });
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _notificationService.CreateAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.MarkAsReadAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.MarkAllAsReadAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _notificationService.RemoveAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
