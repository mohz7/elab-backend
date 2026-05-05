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
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<PatientProfile?> GetPatientByBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(o => o.PatientProfile)
                .FirstOrDefaultAsync(o => o.Id == bookingId);

            return booking?.PatientProfile;
        }
        public async Task<List<Booking>> GetBookingByPatientAsync(string patientId)
        {
            return await _context.Bookings.Where(o => o.PatientProfileId == patientId).ToListAsync();
        }
        public async Task<List<Booking>> GetByStatusAsync(Status status)
        {
            return await _context.Bookings.Where(o => o.Status == status)
                .OrderByDescending(o => o.BookingDate).ToListAsync();
        }
        public async Task<bool> ChangeStatusAsync(int orderId, Status newStatus)
        {
            var booking = await _context.Bookings.FindAsync(orderId);
            if (booking is null) return false;
            booking.Status = newStatus;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(bo => bo.Branch)
                .Include(bo => bo.StaffProfile)
                    .ThenInclude(sp => sp.User)
                .Include(bo => bo.BookingItems)
                    .ThenInclude(bi => bi.TestCatalog)
                .Include(bo => bo.BookingItems)
                    .ThenInclude(bi => bi.Offer)
                .FirstOrDefaultAsync(bo => bo.Id == bookingId);
        }
    }
}
