using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class AIConversationResponse
    {
        public AIChatMessageResponse UserMessage { get; set; }
        public AIChatMessageResponse AIResponse { get; set; }
    }
}
