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
    public class NotificationsController : ControllerBase
    {
        private INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _notificationService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetById([FromRoute] string userId)
        {
            var result = await _notificationService.GetAllByUserAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetById{id}/{userId}")]
        public async Task<IActionResult> GetById([FromRoute] int id, [FromRoute] string userId)
        {
            var result = await _notificationService.GetByIdAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Unread/{userId}")]
        public async Task<IActionResult> GetUnreadCount([FromRoute] string userId)
        {
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

        [HttpDelete("Remove/{id}/{userId}")]
        public async Task<IActionResult> Remove([FromRoute] int id, [FromRoute] string userId)
        {
            var result = await _notificationService.RemoveAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
