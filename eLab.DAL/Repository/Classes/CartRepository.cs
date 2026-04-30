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
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var items = _context.Carts.Where(c => c.UserId == userId).ToList();
            _context.Carts.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Cart>> GetUserCartAsync(string UserId)
        {
            return await _context.Carts
                .Include(c => c.TestCatalog)
                    .ThenInclude(t => t.Prices)
                .Where(c => c.UserId == UserId)
                .ToListAsync();
        }
    }
}
