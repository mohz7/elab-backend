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
    public interface IPriceService
    {
        public Task<ServiceResult<List<PriceResponse>>> GetAllAsync(int? branchId);
        public Task<ServiceResult<PriceResponse>> GetByIdAsync(int id);
        public Task<ServiceResult<string>> CreateAsync(PriceRequest request, string adminId);
        public Task<ServiceResult<string>> UpdateAsync(int id, PriceUpdateRequest request);
        public Task<ServiceResult<string>> RemoveAsync(int id);
    }
}
