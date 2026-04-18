using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IAIChatService
    {
        Task<ServiceResult<AIChatSessionResponse>> StartSessionAsync(int resultId, string patientId);
        Task<ServiceResult<AIChatSessionResponse>> GetSessionAsync(int aiChatId, string patientId);
        Task<ServiceResult<List<AIChatSessionResponse>>> GetMySessionsAsync(string patientId);
        Task<ServiceResult<AIConversationResponse>> SendMessageAsync(int aiChatId, string patientId, string message);
    }
}
