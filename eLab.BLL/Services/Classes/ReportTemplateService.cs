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
    public class ReportTemplateService : IReportTemplateService
    {
        private readonly IReportTemplateRepository _reportTemplateRepository;

        public ReportTemplateService(IReportTemplateRepository reportTemplateRepository)
        {
            _reportTemplateRepository = reportTemplateRepository;
        }

        public async Task<ServiceResult<string>> CreateAsync(ReportTemplateRequest request, string adminId)
        {
            var reportTemplate = request.Adapt<ReportTemplate>();
            reportTemplate.CreatedById = adminId;
            var result = await _reportTemplateRepository.CreateAsync(reportTemplate);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Created failed", "...");

            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<List<ReportTemplateResponse>>> GetAllAsync(int? testCatalogId)
        {
            var reportTemplates = await _reportTemplateRepository.GetAllAsync();
            if (!reportTemplates.Any())
                return ServiceResult<List<ReportTemplateResponse>>.Fail(404, "There is no ReportTemplate", "...");
            if (testCatalogId.HasValue)
                reportTemplates = reportTemplates.Where(re => re.TestCatalogId == testCatalogId).ToList();
            var result = reportTemplates.Adapt<List<ReportTemplateResponse>>();
            return ServiceResult<List<ReportTemplateResponse>>.Ok(result);
        }

        public async Task<ServiceResult<ReportTemplateResponse>> GetByIdAsync(int id)
        {
            var reportTemplate = await _reportTemplateRepository.GetByIdAsync(id);
            if (reportTemplate is null)
                return ServiceResult<ReportTemplateResponse>.Fail(404, "ReportTemplate not found", "...");

            var result = reportTemplate.Adapt<ReportTemplateResponse>();
            return ServiceResult<ReportTemplateResponse>.Ok(result);
        }

        public async Task<ServiceResult<string>> RemoveAsync(int id)
        {
            var reportTemplate = await _reportTemplateRepository.GetByIdAsync(id);
            if (reportTemplate is null)
                return ServiceResult<string>.Fail(404, "ReportTemplate not found", "...");

            var result = await _reportTemplateRepository.RemoveAsync(reportTemplate);
            if (result != 1)
                return ServiceResult<string>.Fail(401, "Delete failed", "...");

            return ServiceResult<string>.Ok("Deleted successfully");
        }

        public async Task<ServiceResult<string>> UpdateAsync(int id, ReportTemplateUpdateRequest request)
        {
            var reportTemplate = await _reportTemplateRepository.GetByIdAsync(id);
            if (reportTemplate is null)
                return ServiceResult<string>.Fail(404, "ReportTemplate not found", "...");

            request.Adapt(reportTemplate);
            var result = await _reportTemplateRepository.UpdateAsync(reportTemplate);
            if (result != 1)
                return ServiceResult<string>.Fail(401, "Update failed", "...");

            return ServiceResult<string>.Ok("Update is successfully");
        }
    }
}
