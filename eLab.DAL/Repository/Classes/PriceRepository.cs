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
    public class PriceRepository : IPriceRepository
    {
        private readonly ApplicationDbContext _context;

        public PriceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Price price)
        {
            _context.Prices.Add(price);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Price>> GetAllAsync()
        {
            return await _context.Prices.ToListAsync();
        }

        public async Task<Price> GetByIdAsync(int id)
        {
            return await _context.Prices.FindAsync(id);
        }

        public async Task<int> RemoveAsync(Price price)
        {
            _context.Prices.Remove(price);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Price price)
        {
            _context.Prices.Update(price);
            return await _context.SaveChangesAsync();
        }
    }
}
