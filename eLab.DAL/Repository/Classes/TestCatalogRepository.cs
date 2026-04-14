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
    public class TestCatalogRepository : ITestCatalogRepository
    {
        private readonly ApplicationDbContext _context;

        public TestCatalogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(TestCatalog testCatalog)
        {
            _context.TestCatalogs.Add(testCatalog);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<TestCatalog>> GetAllAsync()
        {
            return await _context.TestCatalogs.ToListAsync();
        }

        public async Task<TestCatalog> GetByIdAsync(int id)
        {
            return await _context.TestCatalogs.FindAsync(id);
        }

        public async Task<int> UpdateAsync(TestCatalog testCatalog)
        {
            _context.TestCatalogs.Update(testCatalog);
            return await _context.SaveChangesAsync();
        }
    }
}
