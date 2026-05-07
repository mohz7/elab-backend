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
    public class PatientRecordService : IPatientRecordService
    {
        private readonly IPatientRecordRepository _patientRecordRepository;

        public PatientRecordService(IPatientRecordRepository patientRecordRepository)
        {
            _patientRecordRepository = patientRecordRepository;
        }

        public async Task<ServiceResult<List<PatientRecordResponse>>> GetAllAsync()
        {
            var records = await _patientRecordRepository.GetAllAsync();
            if (!records.Any())
                return ServiceResult<List<PatientRecordResponse>>.Fail(404, "Not any PatientRecords", "...");

            var grouped = records
                .GroupBy(r => r.PatientProfileId)
                .Select(g => new PatientRecordResponse
                {
                    PatientId = g.Key,
                    PatientName = g.First().PatientProfile?.User?.FullName ?? "",
                    Email = g.First().PatientProfile?.User?.Email ?? "",
                    PhoneNumber = g.First().PatientProfile?.User?.PhoneNumber ?? "",
                    Records = g.Select(r => new PatientRecordItemResponse
                    {
                        Id = r.Id,
                        BookingId = r.BookingId,
                        BookingDate = r.Booking?.BookingDate ?? default,
                        ResultId = r.ResultId,
                        TestName = r.Result?.BookingItem?.TestCatalog?.Name ?? "",
                        ResultStatus = r.Result?.Status ?? ResultStatus.Pending,
                        BranchId = r.BranchId,
                        BranchName = r.Branch?.Name ?? "",
                        CreatedAt = r.CreatedAt
                    }).ToList()
                }).ToList();

            return ServiceResult<List<PatientRecordResponse>>.Ok(grouped);
        }

        public async Task<ServiceResult<PatientRecordResponse>> GetByIdAsync(int id)
        {
            var r = await _patientRecordRepository.GetByIdAsync(id);
            if (r is null)
                return ServiceResult<PatientRecordResponse>.Fail(404, "PatientRecord not found", "...");

            var result = new PatientRecordResponse
            {
                PatientId = r.PatientProfileId,
                PatientName = r.PatientProfile?.User?.FullName ?? "",
                Email = r.PatientProfile?.User?.Email ?? "",
                PhoneNumber = r.PatientProfile?.User?.PhoneNumber ?? "",
                Records = new List<PatientRecordItemResponse>
        {
            new PatientRecordItemResponse
            {
                Id = r.Id,
                BookingId = r.BookingId,
                BookingDate = r.Booking?.BookingDate ?? default,
                ResultId = r.ResultId,
                TestName = r.Result?.BookingItem?.TestCatalog?.Name ?? "",
                ResultStatus = r.Result?.Status ?? ResultStatus.Pending,
                BranchId = r.BranchId,
                BranchName = r.Branch?.Name ?? "",
                CreatedAt = r.CreatedAt
            }
        }
            };

            return ServiceResult<PatientRecordResponse>.Ok(result);
        }

        public async Task<ServiceResult<PatientRecordResponse>> GetByPatientProfileIdAsync(string patientProfileId)
        {
            var records = await _patientRecordRepository.GetByPatientProfileIdAsync(patientProfileId);
            if (!records.Any())
                return ServiceResult<PatientRecordResponse>.Fail(404, "Not any PatientRecords", "...");

            var result = new PatientRecordResponse
            {
                PatientId = patientProfileId,
                PatientName = records.First().PatientProfile?.User?.FullName ?? "",
                Email = records.First().PatientProfile?.User?.Email ?? "",
                PhoneNumber = records.First().PatientProfile?.User?.PhoneNumber ?? "",
                Records = records.Select(r => new PatientRecordItemResponse
                {
                    Id = r.Id,
                    BookingId = r.BookingId,
                    BookingDate = r.Booking?.BookingDate ?? default,
                    ResultId = r.ResultId,
                    TestName = r.Result?.BookingItem?.TestCatalog?.Name ?? "",
                    ResultStatus = r.Result?.Status ?? ResultStatus.Pending,
                    BranchId = r.BranchId,
                    BranchName = r.Branch?.Name ?? "",
                    CreatedAt = r.CreatedAt
                }).ToList()
            };

            return ServiceResult<PatientRecordResponse>.Ok(result);
        }
        public async Task<ServiceResult<string>> CreateAsync(PatientRecordRequest request)
        {
            var record = request.Adapt<PatientRecord>();
            var result = await _patientRecordRepository.CreateAsync(record);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to create PatientRecord", "...");

            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<string>> RemoveAsync(int id)
        {
            var record = await _patientRecordRepository.GetByIdAsync(id);
            if (record is null)
                return ServiceResult<string>.Fail(404, "PatientRecord not found", "...");
            var result = await _patientRecordRepository.RemoveAsync(record);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to remove PatientRecord", "...");

            return ServiceResult<string>.Ok("Remove successfully");
        }

        public async Task<ServiceResult<string>> UpdateAsync(int id, PatientRecordRequest request)
        {
            var record = await _patientRecordRepository.GetByIdAsync(id);
            if (record is null)
                return ServiceResult<string>.Fail(404, "PatientRecord not found", "...");

            request.Adapt(record);
            var result = await _patientRecordRepository.UpdateAsync(record);
            if (result < 1)
                return ServiceResult<string>.Fail(401, "Update failed", "...");

            return ServiceResult<string>.Ok("Update is successfully");
        }
    }
}
