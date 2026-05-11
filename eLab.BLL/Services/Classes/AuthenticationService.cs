using eLab.BLL.Services.Interface;
using eLab.DAL.DTO.Requests;
using eLab.DAL.DTO.Responses;
using eLab.DAL.Models;
using Azure.Core;
using eLab.BLL.Services.Interface;
using eLab.DAL.DTO.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using eLab.BLL.Services.Common;
using eLab.DAL.Repository.Interface;
using Microsoft.AspNetCore.WebUtilities;


namespace eLab.BLL.Services.Classes
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<User> _signInManager;
        private readonly IPatientProfileRepository _patientProfileRepository;

        public AuthenticationService(UserManager<User> userManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            SignInManager<User> signInManager,
            IPatientProfileRepository patientProfileRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _patientProfileRepository = patientProfileRepository;
        }

        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            User? user = null;

            if (!string.IsNullOrEmpty(loginRequest.Email))
            {
                user = await _userManager.FindByEmailAsync(loginRequest.Email);
            }
            else if (!string.IsNullOrEmpty(loginRequest.IdentityNumber))
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.IdentityNumber == loginRequest.IdentityNumber);
            }

            if (user is null)
                throw new Exception("Invalid email or IdentityNumber");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);

            if (result.IsLockedOut)
                throw new Exception("Your account is locked");

            if (result.IsNotAllowed)
                throw new Exception("Please confirm your email");

            if (!result.Succeeded)
                throw new Exception("Invalid password");

            return new UserResponse()
            {
                Token = await CreateTokenAsync(user)
            };
        }

        public async Task<string> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return "Email confirmed successfully";

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return $"Email confirmation failed: {errors}";
        }

        public async Task<ServiceResult<string>> RegisterAsync(RegisterRequest registerRequest, HttpRequest Request)
        {
            var user = new User()
            {
                FullName = registerRequest.FullName,
                UserName = registerRequest.UserName,
                PhoneNumber = registerRequest.PhoneNumber,
                Email = registerRequest.Email,
                IdentityNumber = registerRequest.IdentityNumber,
                Gender = registerRequest.Gender,
                DateOfBirth = registerRequest.DateOfBirth,
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));

            // إنشاء الـ PatientProfile
            var patient = new PatientProfile()
            {
                Id = user.IdentityNumber,
                UserId = user.Id,
                BloodType = registerRequest.BloodType,
                ChronicDiseases = registerRequest.ChronicDiseases,
                Allergies = registerRequest.Allergies,
                EmergencyContactName = registerRequest.EmergencyContactName,
                EmergencyContactPhone = registerRequest.EmergencyContactPhone,
                InsuranceCompany = registerRequest.InsuranceCompany,
                InsuranceNumber = registerRequest.InsuranceNumber,
                Notes = registerRequest.Notes,
            };

            var resultPatient = await _patientProfileRepository.CreateAsync(patient);
            if (resultPatient != 1)
                throw new Exception("Failed patient create");

            await _userManager.AddToRoleAsync(user, "Patient");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var baseUrl = _configuration["AppSettings:BaseUrl"];
            var confirmUrl = $"{baseUrl}/api/Identity/Account/ConfirmEmail?token={encodedToken}&userId={user.Id}";

            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email",
                $@"
                <h2>Welcome {user.FullName}</h2>
                <p>Please confirm your email:</p>
                <a href=""{confirmUrl}"">Confirm Email</a>
                <br/><br/>
                <p>If the button doesn't work, copy this link:</p>
                <p>{confirmUrl}</p>
                "
            );

            return ServiceResult<string>.Ok("Please confirm your email");
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("jwtOptions")["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPassword request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new Exception("user not found");

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(5);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(request.Email, "Reset Password", $"<p>code is {code}</p>");
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new Exception("user not found");
            if (user.CodeResetPassword != request.code) return false;
            if (user.PasswordResetCodeExpiry < DateTime.UtcNow) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(request.Email, "Change Password", "<h1>Your password has been changed</h1>");
            }
            return true;
        }
    }
}