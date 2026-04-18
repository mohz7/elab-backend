using eLab.BLL.Services.Interface;
using eLab.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class AIChatRepository : IAIChatRepository
    {
        private readonly ApplicationDbContext _context;
        public AIChatRepository(ApplicationDbContext context) => _context = context;

        public async Task<AIChat?> GetByIdAsync(int aiChatId)
            => await _context.AIChats
                .Include(c => c.AIChatMessages.OrderBy(m => m.SentAt))
                .FirstOrDefaultAsync(c => c.Id == aiChatId);

        public async Task<IEnumerable<AIChat>> GetByPatientIdAsync(string patientId)
            => await _context.AIChats
                .Where(c => c.PatientProfileId == patientId)
                .OrderByDescending(c => c.StartedAt)
                .ToListAsync();

        public async Task<IEnumerable<AIChatMessage>> GetMessagesAsync(int aiChatId)
            => await _context.AIChatMessages
                .Where(m => m.AIChatId == aiChatId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

        public async Task<int> AddSessionAsync(AIChat chat)
        {
            await _context.AIChats.AddAsync(chat);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddMessageAsync(AIChatMessage message)
        {
            await _context.AIChatMessages.AddAsync(message);
            return await _context.SaveChangesAsync();
        }
    }
}
