using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface INotificationRepository
    {
        public Task<List<Notification>> GetAllByUserAsync(string userId);
        public Task<List<Notification>> GetAllAsync();
        public Task<Notification?> GetByIdAsync(int id, string userId);
        public Task<int> GetUnreadCountAsync(string userId);
        public Task<int> CreateAsync(Notification notification);
        public Task<int> UpdateAsync(Notification notification);
        public Task<int> MarkAllAsReadAsync(string userId);
        public Task<int> RemoveAsync(Notification notification);
    }
}
