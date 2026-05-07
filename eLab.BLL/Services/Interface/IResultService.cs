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
    public interface IResultService
    {
        Task<ServiceResult<ResultResponse>> UploadResultAsync(
            UploadResultRequest dto, string staffUserId);

        Task<ServiceResult<ResultResponse>> GetByIdAsync(
            int resultId, string requestingUserId);

        Task<ServiceResult<List<ResultSummaryResponse>>> GetMyResultsAsync(
            string patientProfileId);

        Task<ServiceResult<List<ResultSummaryResponse>>> GetPendingApprovalAsync(
            string staffUserId);
        Task<ServiceResult<List<ResultSummaryResponse>>> GetPendingApprovalInAdminAsync(
            int? branchId);

        Task<ServiceResult<ResultResponse>> ReviewResultAsync(
            int resultId, ReviewResultRequest dto, string staffUserId);
        Task<ServiceResult<List<ResultResponse>>> GetAll(string? userId, int? branchId);
        Task<ServiceResult<List<ResultResponse>>> GetAll(string userId);
    }
}
