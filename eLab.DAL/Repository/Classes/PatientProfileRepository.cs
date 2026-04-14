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
    public class PatientProfileRepository : IPatientProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(PatientProfile patientProfile)
        {
            _context.PatientProfiles.Add(patientProfile);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<PatientProfile>> GetAllAsync()
        {
            return await _context.PatientProfiles.Include(pa => pa.User).ToListAsync();
        }

        public async Task<PatientProfile> GetByIdAsync(string id)
        {
            return await _context.PatientProfiles.Include(pa => pa.User).FirstOrDefaultAsync(pa => pa.Id == id);
        }

        public async Task<int> UpdateAsync(PatientProfile patientProfile)
        {
            _context.PatientProfiles.Update(patientProfile);
            return await _context.SaveChangesAsync();
        }
    }
}
