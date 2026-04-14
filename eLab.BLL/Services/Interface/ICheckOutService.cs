using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface ICheckOutService
    {
        Task<ServiceResult<CheckOutResponse>> ProcessPaymentAsync(CheckOutRequest request, string userId, HttpRequest Request);

        Task<ServiceResult<bool>> HandlePaymentSuccessAsync(int bookingId);
    }
}
