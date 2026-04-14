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
    public class ResultRepository : IResultRepository
    {
        private readonly ApplicationDbContext _context;

        public ResultRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result?> GetByIdAsync(int resultId)
           => await _context.Results
               .Include(r => r.PatientProfile)
                   .ThenInclude(p => p.User)
               .Include(r => r.ReportTemplate)
               .Include(r => r.UploadedBy)
               .Include(r => r.ApprovedBy)
               .Include(r => r.BookingItem)
                   .ThenInclude(bi => bi.TestCatalog)
               .Include(r => r.BookingItem)
                   .ThenInclude(bi => bi.Booking)
                       .ThenInclude(b => b.Branch)
               .FirstOrDefaultAsync(r => r.Id == resultId);

        public async Task<Result?> GetByBookingItemIdAsync(int bookingItemId)
            => await _context.Results
                .Include(r => r.ReportTemplate)
                .FirstOrDefaultAsync(r => r.BookingItemId == bookingItemId);

        public async Task<IEnumerable<Result>> GetByPatientIdAsync(string patientProfileId)
            => await _context.Results
                .Where(r => r.PatientProfileId == patientProfileId
                         && r.Status == ResultStatus.Approved)
                .Include(r => r.ReportTemplate)
                .Include(r => r.BookingItem)
                    .ThenInclude(bi => bi.TestCatalog)
                .OrderByDescending(r => r.ResultDate)
                .ToListAsync();

        public async Task<IEnumerable<Result>> GetPendingApprovalAsync(int branchId)
            => await _context.Results
                .Where(r => r.Status == ResultStatus.Pending
                         && r.BookingItem.Booking.BranchId == branchId)
                .Include(r => r.PatientProfile)
                    .ThenInclude(p => p.User)
                .Include(r => r.BookingItem)
                    .ThenInclude(bi => bi.TestCatalog)
                .Include(r => r.UploadedBy)
                .OrderBy(r => r.UploadedAt)
                .ToListAsync();

        public async Task<bool> ResultExistsForBookingItemAsync(int bookingItemId)
            => await _context.Results
                .AnyAsync(r => r.BookingItemId == bookingItemId);

        public async Task<int> AddAsync(Result result)
        {
            await _context.Results.AddAsync(result);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Result result)
        {
            _context.Results.Update(result);
            return await _context.SaveChangesAsync();
        }
    }
}
