using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Data;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Classes
{
    public class ReferenceRangeRepository : IReferenceRangeRepository
    {
        private readonly ApplicationDbContext _context;

        public ReferenceRangeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(ReferenceRange referenceRange)
        {
            _context.ReferenceRanges.Add(referenceRange);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ReferenceRange>> GetAllAsync()
        {
            return await _context.ReferenceRanges.ToListAsync();
        }

        public async Task<ReferenceRange> GetByIdAsync(int id)
        {
            return await _context.ReferenceRanges.FindAsync(id);
        }

        public async Task<List<ReferenceRange>> GetByTemplateIdAsync(int reportTemplateId, int age, Gender gender)
        {
            return await _context.ReferenceRanges
                .Where(re =>
                    re.ReportTemplateId == reportTemplateId &&
                    re.Gender == gender &&
                    re.AgeMin <= age &&
                    re.AgeMax >= age)
                .OrderByDescending(re => re.Id)  
                .ToListAsync()
                .ContinueWith(t => t.Result
                    .GroupBy(re => re.FieldName, StringComparer.OrdinalIgnoreCase)
                    .Select(g => g.First()) 
                    .ToList());
        }

        public async Task<int> RemoveAsync(ReferenceRange referenceRange)
        {
            _context.ReferenceRanges.Remove(referenceRange);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(ReferenceRange referenceRange)
        {
            _context.ReferenceRanges.Update(referenceRange);
            return await _context.SaveChangesAsync();
        }
    }
}
