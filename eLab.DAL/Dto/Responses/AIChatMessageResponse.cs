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
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public int AIChatId { get; set; }
        public string? SenderId { get; set; }
        public string? SenderName { get; set; }
    }
}
