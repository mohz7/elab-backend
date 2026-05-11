using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IReferenceRangeService
    {
        public Task<ServiceResult<List<ReferenceRangeResponse>>> GetAllAsync(int? ReportTemplateId);
        public Task<ServiceResult<ReferenceRangeResponse>> GetByIdAsync(int id);
        public Task<ServiceResult<string>> CreateAsync(ReferenceRangeRequest request, string adminId);
        public Task<ServiceResult<string>> RemoveAsync(int id);
        public Task<ServiceResult<string>> UpdateAsync(int id, ReferenceRangeUpdateRequest request);
    }
}
