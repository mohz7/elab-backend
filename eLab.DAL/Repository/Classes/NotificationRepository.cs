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
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<List<Notification>> GetAllByUserAsync(string userId)
        {
            return await _context.Notifications.Where(no => no.UserId == userId).ToListAsync();
        }
        public async Task<Notification?> GetByIdAsync(int id, string userId)
        {
            return await _context.Notifications
                .Include(n => n.Result)
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        }
        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
        public async Task<int> CreateAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            notifications.ForEach(n => n.IsRead = true);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveAsync(Notification notification)
        {
            _context.Notifications.Remove(notification);
            return await _context.SaveChangesAsync();
        }
    }
}
