using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface ICartService
    {
        public Task<ServiceResult<string>> AddToCartAsync(CartRequest request, string UserId);
        public Task<ServiceResult<CartSummaryRespones>> CartSummaryResponesAsync(string UserId);
        public Task<ServiceResult<string>> ClearCartAsync(string userId);
    }
}
