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

        public ResultService(IResultRepository resultRepository,
            IBookingItemRepository bookingItemRepository,
            IPatientProfileRepository patientProfileRepository,
            IReferenceRangeRepository referenceRangeRepository,
            IPatientRecordRepository patientRecordRepository,
            INotificationRepository notificationRepository,
            IStaffProfileRepository staffProfileRepository,
            UserManager<User> userManager)
        {
            _resultRepository = resultRepository;
            _bookingItemRepository = bookingItemRepository;
            _patientProfileRepository = patientProfileRepository;
            _referenceRangeRepository = referenceRangeRepository;
            _patientRecordRepository = patientRecordRepository;
            _notificationRepository = notificationRepository;
            _staffProfileRepository = staffProfileRepository;
            _userManager = userManager;
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
                UploadedAt = DateTime.UtcNow
            };

            await _resultRepository.AddAsync(result);

            await _patientRecordRepository.CreateAsync(new PatientRecord
            {
                PatientProfileId = patient.Id,
                ResultId = result.Id,
                BranchId = bookingItem.Booking.BranchId,
                BookingId = bookingItem.BookingId,
                CreatedAt = DateTime.UtcNow
            });

            var response = await BuildResponseDto(result, ranges);
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

            if (result.UploadedById == staffUserId)
                return ServiceResult<ResultResponse>.Fail(403, "You cannot approve your own uploaded result.", "...");

            if (dto.Action == "approve")
            {
                result.Status = ResultStatus.Approved;
                result.ApprovedById = staffUserId;
                result.ReviewedAt = DateTime.UtcNow;

                // 🔥 حذفنا VisibleToPatient

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

            var ranges = await _referenceRangeRepository
                .GetByTemplateIdAsync(result.ReportTemplateId!.Value,
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

            // ✅ المستخدم اللي طلب
            var requestingUser = await _userManager.FindByIdAsync(requestingUserId);

            if (requestingUser == null)
                return ServiceResult<ResultResponse>.Fail(404, "User not found.", "...");

            // ✅ تحقق من الدور
            var isPatient = await _userManager.IsInRoleAsync(requestingUser, "Patient");

            if (isPatient)
            {
                if (result.PatientProfile.UserId != requestingUserId)
                    return ServiceResult<ResultResponse>.Fail(403, "Access denied.", "...");

                if (result.Status != ResultStatus.Approved)
                    return ServiceResult<ResultResponse>.Fail(403, "This result is not yet available.", "...");
            }

            var ranges = await _referenceRangeRepository.GetByTemplateIdAsync(
                result.ReportTemplateId!.Value,
                result.PatientProfile.User.Age,
                result.PatientProfile.User.Gender);

            var response = await BuildResponseDto(result, ranges);
            return ServiceResult<ResultResponse>.Ok(response);
        }

        // ── Get My Results (Patient) ──────────────────────────────
        public async Task<ServiceResult<List<ResultSummaryResponse>>> GetMyResultsAsync(
            string patientProfileId)
        {
            var results = await _resultRepository.GetByPatientIdAsync(patientProfileId);

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
                    HasAbnormalValues = abnormalCount > 0
                };
            }).ToList();

            return ServiceResult<List<ResultSummaryResponse>>.Ok(response);
        }

        // ── Get Pending Approval (Staff) ──────────────────────────
        public async Task<ServiceResult<List<ResultSummaryResponse>>> GetPendingApprovalAsync(
            string staffUserId)
        {
            var staff = await _staffProfileRepository.GetByIdAsync(staffUserId);
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
                HasAbnormalValues = false
            }).ToList();

            return ServiceResult<List<ResultSummaryResponse>>.Ok(response);
        }

        // ── Flag Evaluation ───────────────────────────────────────
        private ResultFlags EvaluateOverallFlag(
            Dictionary<string, decimal> resultData,
            IEnumerable<ReferenceRange> ranges)
        {
            bool hasLow = false;

            var rangeDict = ranges.ToDictionary(
                r => r.FieldName,
                StringComparer.OrdinalIgnoreCase);

            foreach (var (paramName, value) in resultData)
            {
                if (!rangeDict.TryGetValue(paramName, out var range))
                    continue;

                if (value > range.ValueMax)
                    return ResultFlags.High;

                if (value < range.ValueMin)
                    hasLow = true;
            }

            return hasLow ? ResultFlags.Low : ResultFlags.Normal;
        }

        // ── Deserialize Flags ─────────────────────────────────────
        private Dictionary<string, ResultFlags> DeserializeFlags(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return new Dictionary<string, ResultFlags>();

            var raw = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                      ?? new Dictionary<string, string>();

            return raw.ToDictionary(kvp => kvp.Key, kvp => Enum.Parse<ResultFlags>(kvp.Value));
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

                return new ResultParameterResponse
                {
                    ParameterName = kvp.Key,
                    Value = kvp.Value,
                    Flag = result.ResultFlags,
                    RangeMin = range?.ValueMin,
                    RangeMax = range?.ValueMax,
                    Unit = range?.Units
                };
            }).ToList();

            var abnormalCount = result.ResultFlags == ResultFlags.Normal ? 0 : 1;

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
                AbnormalCount = abnormalCount
            };
        }
    }
}
