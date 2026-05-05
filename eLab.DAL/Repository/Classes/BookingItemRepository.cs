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
    public class BookingItemRepository : IBookingItemRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(List<BookingItem> items)
        {
            await _context.BookingItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public async Task<BookingItem> GetByIdAsync(int id)
        {
            return await _context.BookingItems
                .Include(bi => bi.Booking) // ✅
                .Include(bi => bi.TestCatalog)
                .Include(bi => bi.Offer)
                .FirstOrDefaultAsync(bi => bi.Id == id);
        }
    }
}
