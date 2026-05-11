using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using eLab.DAL.Models;

namespace eLab.DAL.Repository.Interface
{
    public interface IStaffChatRepository
    {
        Task<StaffChat?> GetByIdAsync(int chatId);
        Task<StaffChat?> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<StaffChat>> GetByPatientIdAsync(string patientId);
        Task<IEnumerable<StaffChat>> GetByStaffIdAsync(string staffId);
        Task<IEnumerable<StaffChatMessage>> GetMessagesAsync(int chatId);
        Task<bool> SessionExistsForResultAsync(int resultId);   // ← جديد
        Task<int> AddSessionAsync(StaffChat chat);
        Task<int> AddMessageAsync(StaffChatMessage message);
        Task MarkMessagesAsReadAsync(int chatId, string readerId);
    }
}
