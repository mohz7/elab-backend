using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Classes;
using eLab.DAL.Repository.Interface;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class ReferenceRangeService : IReferenceRangeService
    {
        private readonly IReferenceRangeRepository _referenceRangeRepository;

        public ReferenceRangeService(IReferenceRangeRepository referenceRangeRepository)
        {
            _referenceRangeRepository = referenceRangeRepository;
        }

        public async Task<ServiceResult<string>> CreateAsync(ReferenceRangeRequest request, string adminId)
        {
            var referenceRange = request.Adapt<ReferenceRange>();
            referenceRange.CreatedById = adminId;
            var result = await _referenceRangeRepository.CreateAsync(referenceRange);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Created failed", "...");

            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<List<ReferenceRangeResponse>>> GetAllAsync(int? ReportTemplateId)
        {
            var referenceRanges = await _referenceRangeRepository.GetAllAsync();
            if (!referenceRanges.Any())
                return ServiceResult<List<ReferenceRangeResponse>>.Fail(404, "There is no ReferenceRanges", "...");
            if (ReportTemplateId.HasValue)
                referenceRanges = referenceRanges.Where(re => re.ReportTemplateId == ReportTemplateId).ToList();
            var result = referenceRanges.Adapt<List<ReferenceRangeResponse>>();
            return ServiceResult<List<ReferenceRangeResponse>>.Ok(result);
        }

        public async Task<ServiceResult<ReferenceRangeResponse>> GetByIdAsync(int id)
        {
            var referenceRange = await _referenceRangeRepository.GetByIdAsync(id);
            if (referenceRange is null)
                return ServiceResult<ReferenceRangeResponse>.Fail(404, "ReferenceRange not found", "...");

            var result = referenceRange.Adapt<ReferenceRangeResponse>();
            return ServiceResult<ReferenceRangeResponse>.Ok(result);
        }

        public async Task<ServiceResult<string>> RemoveAsync(int id)
        {
            var referenceRange = await _referenceRangeRepository.GetByIdAsync(id);
            if (referenceRange is null)
                return ServiceResult<string>.Fail(404, "ReferenceRange not found", "...");

            var result = await _referenceRangeRepository.RemoveAsync(referenceRange);
            if (result != 1)
                return ServiceResult<string>.Fail(401, "Delete failed", "...");

            return ServiceResult<string>.Ok("Deleted successfully");
        }

        public async Task<ServiceResult<string>> UpdateAsync(int id, ReferenceRangeUpdateRequest request)
        {
            var referenceRange = await _referenceRangeRepository.GetByIdAsync(id);
            if (referenceRange is null) 
                return ServiceResult<string>.Fail(404, "ReferenceRange not found", "...");

            request.Adapt(referenceRange);
            var result = await _referenceRangeRepository.UpdateAsync(referenceRange);
            if(result != 1)
                return ServiceResult<string>.Fail(401, "Update failed", "...");

            return ServiceResult<string>.Ok("Update is successfully");
        }
    }
}
