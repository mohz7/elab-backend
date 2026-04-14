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
    public interface IReportTemplateService
    {
        public Task<ServiceResult<List<ReportTemplateResponse>>> GetAllAsync(int? testCatalogId);
        public Task<ServiceResult<ReportTemplateResponse>> GetByIdAsync(int id);
        public Task<ServiceResult<string>> CreateAsync(ReportTemplateRequest request, string adminId);
        public Task<ServiceResult<string>> RemoveAsync(int id);
        public Task<ServiceResult<string>> UpdateAsync(int id, ReportTemplateRequest request);
    }
}
