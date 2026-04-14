using eLab.DAL.DTO.Requests;
using eLab.DAL.DTO.Responses;
using eLab.DAL.DTO.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLab.BLL.Services.Common;

namespace eLab.BLL.Services.Interface
{
    public interface IAuthenticationService
    {
       Task<UserResponse> LoginAsync(LoginRequest loginRequest);
       Task<ServiceResult<string>> RegisterAsync(RegisterRequest registerRequest, HttpRequest Request);
       Task<string> ConfirmEmail(string token , string userId );
        Task<bool> ForgotPasswordAsync(ForgotPassword request);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest request);


    }
}
