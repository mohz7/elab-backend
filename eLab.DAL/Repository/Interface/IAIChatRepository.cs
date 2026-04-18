using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IAIChatRepository
    {
        Task<AIChat?> GetByIdAsync(int aiChatId);
        Task<IEnumerable<AIChat>> GetByPatientIdAsync(string patientId);
        Task<IEnumerable<AIChatMessage>> GetMessagesAsync(int aiChatId);
        Task<int> AddSessionAsync(AIChat chat);
        Task<int> AddMessageAsync(AIChatMessage message);
    }
}
