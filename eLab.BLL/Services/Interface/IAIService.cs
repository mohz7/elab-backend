using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    // BLL/Interfaces/IAIService.cs
    public interface IAIService
    {
        // called by AIChatService.SendMessageAsync
        // takes the result context + full conversation history + new message
        // returns the AI generated reply as a plain string
        Task<string> GetConversationResponseAsync(
            string contextSummary,
            IEnumerable<AIChatMessage> history,
            string newMessage
        );
    }
}
