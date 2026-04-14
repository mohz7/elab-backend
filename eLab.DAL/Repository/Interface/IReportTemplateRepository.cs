using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IReportTemplateRepository
    {
        public Task<List<ReportTemplate>> GetAllAsync();
        public Task<ReportTemplate> GetByIdAsync(int id);
        public Task<int> CreateAsync(ReportTemplate reportTemplate);
        public Task<int> RemoveAsync(ReportTemplate reportTemplate);
        public Task<int> UpdateAsync(ReportTemplate reportTemplate);
    }
}
