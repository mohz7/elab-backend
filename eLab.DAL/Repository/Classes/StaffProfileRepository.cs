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
    public class StaffProfileRepository : IStaffProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public StaffProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(StaffProfile staffProfile)
        {
            _context.StaffProfiles.Add(staffProfile);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<StaffProfile>> GetAllAsync()
        {
            return await _context.StaffProfiles.Include(st => st.Branch).Include(st => st.User).Include(st => st.CreatedBy).ToListAsync();
        }

        public async Task<StaffProfile> GetByIdAsync(string id)
        {
            return await _context.StaffProfiles.Include(st => st.Branch).Include(st => st.User).Include(st => st.CreatedBy).FirstOrDefaultAsync(st => st.Id == id);
        }
        public async Task<int> UpdateAsync(StaffProfile staffProfile)
        {
            _context.StaffProfiles.Update(staffProfile);
            return await _context.SaveChangesAsync();
        }
    }
}
