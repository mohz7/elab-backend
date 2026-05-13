using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Midicare_eLab.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace eLab.BLL.Services.Classes
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IBookingItemRepository _bookingItemRepository;
        private readonly IPatientProfileRepository _patientProfileRepository;
        private readonly IReferenceRangeRepository _referenceRangeRepository;
        private readonly IPatientRecordRepository _patientRecordRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IStaffProfileRepository _staffProfileRepository;
        private readonly UserManager<User> _userManager;
        private readonly IBranchRepository _branchRepository;

        public ResultService(IResultRepository resultRepository,
            IBookingItemRepository bookingItemRepository,
            IPatientProfileRepository patientProfileRepository,
            IReferenceRangeRepository referenceRangeRepository,
            IPatientRecordRepository patientRecordRepository,
            INotificationRepository notificationRepository,
            IStaffProfileRepository staffProfileRepository,
            UserManager<User> userManager,
            IBranchRepository branchRepository)
        {
            _resultRepository = resultRepository;
            _bookingItemRepository = bookingItemRepository;
            _patientProfileRepository = patientProfileRepository;
            _referenceRangeRepository = referenceRangeRepository;
            _patientRecordRepository = patientRecordRepository;
            _notificationRepository = notificationRepository;
            _staffProfileRepository = staffProfileRepository;
            _userManager = userManager;
            _branchRepository = branchRepository;
        }

        // ── Upload ────────────────────────────────────────────────
        public async Task<ServiceResult<ResultResponse>> UploadResultAsync(
            UploadResultRequest dto, string staffUserId)
        {
            var bookingItem = await _bookingItemRepository.GetByIdAsync(dto.BookingItemId);
            if (bookingItem == null)
                return ServiceResult<ResultResponse>.Fail(404, "Booking item not found.", "...");

            if (await _resultRepository.ResultExistsForBookingItemAsync(dto.BookingItemId))
                return ServiceResult<ResultResponse>.Fail(400, "A result already exists for this booking item.", "...");

            var patient = await _patientProfileRepository.GetByIdAsync(bookingItem.Booking.PatientProfileId);
            if (patient == null)
                return ServiceResult<ResultResponse>.Fail(404, "Patient profile not found.", "...");

            if (bookingItem.Booking.Status != Status.Confirmed)
                return ServiceResult<ResultResponse>.Fail(400, "Cannot upload result for a booking that is not confirmed.", "...");

            var ranges = await _referenceRangeRepository
                .GetByTemplateIdAsync(dto.ReportTemplateId, patient.User.Age, patient.User.Gender);

            var overallFlag = EvaluateOverallFlag(dto.ResultData, ranges);
            var resultDataJson = JsonSerializer.Serialize(dto.ResultData);

            var result = new Result
            {
                BookingItemId = dto.BookingItemId,
                PatientProfileId = patient.Id,
                ReportTemplateId = dto.ReportTemplateId,
                UploadedById = staffUserId,
                ResultDate = dto.ResultDate,
                ResultData = resultDataJson,
                ResultFlags = overallFlag,
                Status = ResultStatus.Pending,
                FileUrl = dto.FileUrl,
                UploadedAt = DateTime.UtcNow,
            };

            await _resultRepository.AddAsync(result);

            await _patientRecordRepository.CreateAsync(new PatientRecord
            {
                PatientProfileId = patient.Id,
                ResultId = result.Id,
                BranchId = bookingItem.Booking.BranchId,
                BookingId = bookingItem.BookingId,
                CreatedAt = DateTime.UtcNow,
            });

            var savedResult = await _resultRepository.GetByIdAsync(result.Id);

            var response = await BuildResponseDto(savedResult!, ranges);
            return ServiceResult<ResultResponse>.Ok(response);
        }

        // ── Review (Approve / Reject) ─────────────────────────────
        public async Task<ServiceResult<ResultResponse>> ReviewResultAsync(
            int resultId, ReviewResultRequest dto, string staffUserId)
        {
            var result = await _resultRepository.GetByIdAsync(resultId);
            if (result == null)
                return ServiceResult<ResultResponse>.Fail(404, "Result not found.", "...");

            if (result.Status != ResultStatus.Pending)
                return ServiceResult<ResultResponse>.Fail(400, "Only pending results can be reviewed.", "...");

            if (result.UploadedById != staffUserId)
                return ServiceResult<ResultResponse>.Fail(403, "You cannot approve your own uploaded result.", "...");

            if (result.PatientProfile == null)
                return ServiceResult<ResultResponse>.Fail(400, "Patient profile not found.", "...");

            if (dto.Action != ResultStatus.Approved && dto.Action != ResultStatus.Rejected)
                return ServiceResult<ResultResponse>.Fail(400, "Invalid action. Must be Approved or Rejected.", "...");

            try
            {
                if (dto.Action == ResultStatus.Approved)
                {
                    result.Status = ResultStatus.Approved;
                    result.ApprovedById = staffUserId;
                    result.ReviewedAt = DateTime.UtcNow;

                    await _notificationRepository.CreateAsync(new Notification
                    {
                        UserId = result.PatientProfile.UserId,
                        ResultId = result.Id,
                        Type = NotificationType.ResultReady,
                        Message = "Your lab result is ready. You can now view it.",
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    result.Status = ResultStatus.Rejected;
                    result.ReviewedAt = DateTime.UtcNow;
                }

                await _resultRepository.UpdateAsync(result);
            }
            catch (Exception ex)
            {
                return ServiceResult<ResultResponse>.Fail(500, "An error occurred.", ex.Message);
            }

            var ranges = await _referenceRangeRepository.GetByTemplateIdAsync(
                result.ReportTemplateId!.Value,
                result.PatientProfile.User.Age,
                result.PatientProfile.User.Gender);

            var response = await BuildResponseDto(result, ranges);
            return ServiceResult<ResultResponse>.Ok(response);
        }

        // ── Get By Id ─────────────────────────────────────────────
        public async Task<ServiceResult<ResultResponse>> GetByIdAsync(
            int resultId, string requestingUserId)
        {
            var result = await _resultRepository.GetByIdAsync(resultId);

            if (result == null)
                return ServiceResult<ResultResponse>.Fail(404, "Result not found.", "...");

            var requestingUser = await _userManager.FindByIdAsync(requestingUserId);

            if (requestingUser == null)
                return ServiceResult<ResultResponse>.Fail(404, "User not found.", "...");

            var isPatient = await _userManager.IsInRoleAsync(requestingUser, "Patient");

            if (isPatient)
            {
                if (result.PatientProfile!.UserId != requestingUserId)
                    return ServiceResult<ResultResponse>.Fail(403, "Access denied.", "...");

                if (result.Status != ResultStatus.Approved)
                    return ServiceResult<ResultResponse>.Fail(403, "This result is not yet available.", "...");
            }

            var ranges = await _referenceRangeRepository.GetByTemplateIdAsync(
                result.ReportTemplateId!.Value,
                result.PatientProfile!.User.Age,
                result.PatientProfile.User.Gender);

            var response = await BuildResponseDto(result, ranges);
            return ServiceResult<ResultResponse>.Ok(response);
        }

        // ── Get My Results (Patient) ──────────────────────────────
        public async Task<ServiceResult<List<ResultSummaryResponse>>> GetMyResultsAsync(
            string patientProfileId)
        {
            var user = await _userManager.FindByIdAsync(patientProfileId);
            var results = await _resultRepository.GetByPatientIdAsync(user.IdentityNumber);
            if (results is null)
                return ServiceResult<List<ResultSummaryResponse>>.Fail(404, "patient not found", "...");

            var response = results.Select(r =>
            {
                var abnormalCount = r.ResultFlags == ResultFlags.Normal ? 0 : 1;

                return new ResultSummaryResponse
                {
                    Id = r.Id,
                    TestName = r.BookingItem?.TestCatalog?.Name ?? "",
                    ResultDate = r.ResultDate,
                    Status = r.Status,
                    AbnormalCount = abnormalCount,
                    HasAbnormalValues = abnormalCount > 0,
                    UploadedAt = r.UploadedAt
                };
            }).ToList();

            return ServiceResult<List<ResultSummaryResponse>>.Ok(response);
        }

        // ── Get Pending Approval (Staff) ──────────────────────────
        public async Task<ServiceResult<List<ResultSummaryResponse>>> GetPendingApprovalAsync(
            string staffUserId)
        {
            var user = await _userManager.FindByIdAsync(staffUserId);
            var staff = await _staffProfileRepository.GetByIdAsync(user.IdentityNumber);
            if (staff == null)
                return ServiceResult<List<ResultSummaryResponse>>.Fail(404, "Staff profile not found.", "...");

            var results = await _resultRepository.GetPendingApprovalAsync(staff.BranchId);

            var response = results.Select(r => new ResultSummaryResponse
            {
                Id = r.Id,
                TestName = r.BookingItem?.TestCatalog?.Name ?? "",
                ResultDate = r.ResultDate,
                Status = r.Status,
                AbnormalCount = 0,
                HasAbnormalValues = false,
                UploadedAt = r.UploadedAt
            }).ToList();

            return ServiceResult<List<ResultSummaryResponse>>.Ok(response);
        }

        // ── Get Pending Approval (Admin) ──────────────────────────
        public async Task<ServiceResult<List<ResultSummaryResponse>>> GetPendingApprovalInAdminAsync(int? branchId)
        {
            if (branchId.HasValue)
            {
                var branch = await _branchRepository.GetByIdAsync(branchId.Value); 
                if (branch == null)
                    return ServiceResult<List<ResultSummaryResponse>>.Fail(404, "Branch not found.", "...");
            }

            var results = await _resultRepository.GetPendingApprovalAsync(branchId.Value);

            var response = results.Select(r => new ResultSummaryResponse
            {
                Id = r.Id,
                TestName = r.BookingItem?.TestCatalog?.Name ?? "",
                ResultDate = r.ResultDate,
                Status = r.Status,
                AbnormalCount = r.ResultFlags == ResultFlags.Normal ? 0 : 1, 
                HasAbnormalValues = r.ResultFlags != ResultFlags.Normal,       
                UploadedAt = r.UploadedAt
            }).ToList();

            return ServiceResult<List<ResultSummaryResponse>>.Ok(response);
        }

        // ── Get All (Admin) ──────────────────────────
        public async Task<ServiceResult<List<ResultResponse>>> GetAll(string? userId, int? branchId)
        {
            var results = await _resultRepository.GetAll();
            if (!results.Any())
                return ServiceResult<List<ResultResponse>>.Fail(404, "Result not found", "...");

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.IdentityNumber == userId);
                if (user == null)
                    return ServiceResult<List<ResultResponse>>.Fail(404, "Staff not found.", "...");
                results = results.Where(re => re.UploadedById == user.Id).ToList();
            }

            if (branchId.HasValue)
            {
                var branch = await _branchRepository.GetByIdAsync(branchId.Value);
                if (branch == null)
                    return ServiceResult<List<ResultResponse>>.Fail(404, "Branch not found.", "...");
                results = results.Where(re => re.BookingItem.Booking.BranchId == branchId.Value).ToList();
            }

            if (!results.Any())
                return ServiceResult<List<ResultResponse>>.Fail(404, "No results found for the given filters.", "...");

            var responseList = new List<ResultResponse>();
            foreach (var result in results)
            {
                var ranges = await _referenceRangeRepository.GetByTemplateIdAsync(
                    result.ReportTemplateId!.Value,
                    result.PatientProfile!.User.Age,
                    result.PatientProfile.User.Gender);
                responseList.Add(await BuildResponseDto(result, ranges));
            }

            return ServiceResult<List<ResultResponse>>.Ok(responseList);
        }

        // ── Get All (Staff) ──────────────────────────
        public async Task<ServiceResult<List<ResultResponse>>> GetAll(string userId)
        {
            var results = await _resultRepository.GetAll();
            if (!results.Any())
                return ServiceResult<List<ResultResponse>>.Fail(404, "Result not found", "...");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return ServiceResult<List<ResultResponse>>.Fail(404, "Staff not found.", "...");

            results = results.Where(re => re.UploadedById == user.Id).ToList();

            var responseList = new List<ResultResponse>();

            foreach (var result in results)
            {
                var ranges = await _referenceRangeRepository.GetByTemplateIdAsync(
                    result.ReportTemplateId!.Value,
                    result.PatientProfile!.User.Age,
                    result.PatientProfile.User.Gender);

                responseList.Add(await BuildResponseDto(result, ranges));
            }

            return ServiceResult<List<ResultResponse>>.Ok(responseList);
        }

        // ── Flag Evaluation ───────────────────────────────────────
        private ResultFlags EvaluateOverallFlag(
            Dictionary<string, decimal> resultData,
            IEnumerable<ReferenceRange> ranges)
        {
            bool hasLow = false;
            bool hasHigh = false;

            var rangeDict = ranges.ToDictionary(
                r => r.FieldName,
                StringComparer.OrdinalIgnoreCase);

            foreach (var (paramName, value) in resultData)
            {
                if (!rangeDict.TryGetValue(paramName, out var range))
                    continue;

                if (value > range.ValueMax)
                    hasHigh = true;

                if (value < range.ValueMin)
                    hasLow = true;
            }

            if (hasHigh) return ResultFlags.High;
            if (hasLow) return ResultFlags.Low;
            return ResultFlags.Normal;
        }

        // ── Build Response DTO ────────────────────────────────────
        private async Task<ResultResponse> BuildResponseDto(
            Result result, IEnumerable<ReferenceRange> ranges)
        {
            var resultData = string.IsNullOrEmpty(result.ResultData)
                ? new Dictionary<string, decimal>()
                : JsonSerializer.Deserialize<Dictionary<string, decimal>>(result.ResultData)!;

            var parameters = resultData.Select(kvp =>
            {
                var range = ranges.FirstOrDefault(r =>
                    r.FieldName.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase));

                ResultFlags paramFlag = ResultFlags.Normal;
                if (range != null)
                {
                    if (kvp.Value > range.ValueMax)
                        paramFlag = ResultFlags.High;
                    else if (kvp.Value < range.ValueMin)
                        paramFlag = ResultFlags.Low;
                }

                return new ResultParameterResponse
                {
                    ParameterName = kvp.Key,
                    Value = kvp.Value,
                    Flag = paramFlag, 
                    RangeMin = range?.ValueMin,
                    RangeMax = range?.ValueMax,
                    Unit = range?.Units
                };
            }).ToList();

            // ✅ احسب abnormalCount من الـ parameters الفعلية
            var abnormalCount = parameters.Count(p => p.Flag != ResultFlags.Normal);

            return new ResultResponse
            {
                Id = result.Id,
                TestName = result.BookingItem?.TestCatalog?.Name ?? "",
                TemplateName = result.ReportTemplate?.Name ?? "",
                ResultDate = result.ResultDate,
                UploadedAt = result.UploadedAt,
                ReviewedAt = result.ReviewedAt,
                Status = result.Status,
                FileUrl = result.FileUrl,
                UploadedByName = result.UploadedBy?.FullName ?? "",
                ApprovedByName = result.ApprovedBy?.FullName,
                Parameters = parameters,
                TotalParameters = parameters.Count,
                AbnormalCount = abnormalCount,
                PatientName = result.PatientProfile.User.FullName
            };
        }
    }
}