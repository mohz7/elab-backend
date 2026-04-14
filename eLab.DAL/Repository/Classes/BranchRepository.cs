using eLab.DAL.Dto.Requests;
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
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;

        public BranchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Branch branch)
        {
            await _context.Branches.AddAsync(branch);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Branch>> GetAllPatientAsync()
        {
            return await _context.Branches.Where(p => p.IsActive == true).ToListAsync();
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            return await _context.Branches.FindAsync(id);
        }

        public async Task<List<Branch>> GettAllAsync()
        {
            return await _context.Branches.ToListAsync();
        }

        public async Task<int> RemoveAsync(Branch branch)
        {
            _context.Branches.Remove(branch);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Branch branch)
        {
            _context.Branches.Update(branch);
            return await _context.SaveChangesAsync();
        }
    }
}
