using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Classes
{
    public class ReportTemplateRepository : IReportTemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(ReportTemplate reportTemplate)
        {
            _context.ReportTemplates.Add(reportTemplate);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ReportTemplate>> GetAllAsync()
        {
            return await _context.ReportTemplates.ToListAsync();
        }

        public async Task<ReportTemplate> GetByIdAsync(int id)
        {
            return await _context.ReportTemplates.FindAsync(id);
        }

        public async Task<int> RemoveAsync(ReportTemplate reportTemplate)
        {
            _context.ReportTemplates.Remove(reportTemplate);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(ReportTemplate reportTemplate)
        {
            _context.ReportTemplates.Update(reportTemplate);
            return await _context.SaveChangesAsync();
        }
    }
}
