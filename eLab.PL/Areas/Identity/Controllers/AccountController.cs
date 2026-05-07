using eLab.BLL.Services.Interface;
using eLab.DAL.DTO.Requests;
using eLab.DAL.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace eLab.PL.Areas.Identity.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("identity")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request, Request);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult<string>> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            var result = await _authenticationService.ConfirmEmail(token, userId);
            return Ok(result);
        }
        [HttpPost("forgot-password")]
        public async Task<ActionResult<string>> ForgotPassword([FromBody] ForgotPassword request)
        {
            var result = await _authenticationService.ForgotPasswordAsync(request);
            return Ok(result);
        }
        [HttpPatch("reset-password")]
        public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);
            return Ok(result);
        }
    }
}
