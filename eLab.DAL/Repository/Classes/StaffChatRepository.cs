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
    public class StaffChatRepository : IStaffChatRepository
    {
        private readonly ApplicationDbContext _context;
        public StaffChatRepository(ApplicationDbContext context) => _context = context;

        public async Task<StaffChat?> GetByIdAsync(int chatId)
            => await _context.StaffChats
                .Include(c => c.StaffChatMessages.OrderBy(m => m.SentAt))
                .Include(c => c.PatientProfile)
                .Include(c => c.StaffProfile)
                .FirstOrDefaultAsync(c => c.Id == chatId);

        public async Task<StaffChat?> GetByBookingIdAsync(int bookingId)
            => await _context.StaffChats
                .Include(c => c.StaffChatMessages.OrderBy(m => m.SentAt))
                .FirstOrDefaultAsync(c => c.BookingId == bookingId);

        public async Task<IEnumerable<StaffChat>> GetByPatientIdAsync(string patientId)
            => await _context.StaffChats
                .Where(c => c.PatientProfileId == patientId)
                .Include(c => c.StaffChatMessages)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<StaffChat>> GetByStaffIdAsync(string staffId)
            => await _context.StaffChats
                .Where(c => c.StaffProfileId == staffId)
                .Include(c => c.StaffChatMessages)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<StaffChatMessage>> GetMessagesAsync(int chatId)
            => await _context.StaffChatMessages
                .Where(m => m.ChatId == chatId)
                .Include(m => m.Sender)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

        public async Task<bool> SessionExistsForBookingAsync(int bookingId)
            => await _context.StaffChats.AnyAsync(c => c.BookingId == bookingId);

        public async Task<int> AddSessionAsync(StaffChat chat)
        {
            await _context.StaffChats.AddAsync(chat);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddMessageAsync(StaffChatMessage message)
        {
            await _context.StaffChatMessages.AddAsync(message);
            return await _context.SaveChangesAsync();
        }

        public async Task MarkMessagesAsReadAsync(int chatId, string readerId)
            => await _context.StaffChatMessages
                .Where(m => m.ChatId == chatId && m.SenderId != readerId && !m.IsRead)
                .ExecuteUpdateAsync(m => m.SetProperty(x => x.IsRead, true));
    }
}
