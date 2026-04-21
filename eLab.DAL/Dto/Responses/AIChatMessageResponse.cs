using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class AIChatMessageResponse
    {
        public int Id { get; set; }
        public int AIChatId { get; set; }

        // "user" or "assistant" — matches Gemini/OpenAI convention
        // frontend uses this to align messages left (assistant) or right (user)
        public string Role { get; set; }

        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
