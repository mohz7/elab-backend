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
    public class PatientRecordRepository : IPatientRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(PatientRecord patientRecord)
        {
            _context.PatientRecords.Add(patientRecord);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<PatientRecord>> GetAllAsync()
        {
            return await _context.PatientRecords
                .Include(r => r.PatientProfile).ThenInclude(p => p.User)
                .Include(r => r.Booking)
                .Include(r => r.Result)
                .Include(r => r.Branch)
                .ToListAsync();
        }

        public async Task<PatientRecord> GetByIdAsync(int id)
        {
            return await _context.PatientRecords
                .Include(r => r.PatientProfile).ThenInclude(p => p.User)
                .Include(r => r.Booking)
                .Include(r => r.Result)
                .Include(r => r.Branch)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<PatientRecord>> GetByPatientProfileIdAsync(string patientProfileId)
        {
            return await _context.PatientRecords
                .Include(par => par.PatientProfile)
                .ThenInclude(pa => pa.User)
                .Include(par => par.Booking)
                .Include(par => par.Result).Include(par => par.Branch)
                .Where(par => par.PatientProfileId == patientProfileId)
                .ToListAsync();
        }

        public async Task<int> RemoveAsync(PatientRecord patientRecord)
        {
            _context.PatientRecords.Remove(patientRecord);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(PatientRecord patientRecord)
        {
            _context.PatientRecords.Update(patientRecord);
            return await _context.SaveChangesAsync();
        }
    }
}

