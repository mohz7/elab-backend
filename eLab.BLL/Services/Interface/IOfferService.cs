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
    public interface IOfferService
    {
        public Task<ServiceResult<List<OfferResponse>>> GetAllAsync(int? branchId, bool onlyActive = false);
        public Task<ServiceResult<OfferResponse>> GetByIdAsync(int id);
        public Task<ServiceResult<string>> CreateAsync(OfferRequest request, string adminId);
        public Task<ServiceResult<string>> UpdateAsync(int id, OfferRequest request);
        public Task<ServiceResult<string>> DeactivateTestCatalogAsync(int id);
        public Task<ServiceResult<string>> ActivateTestCatalogAsync(int id);

    }
}
