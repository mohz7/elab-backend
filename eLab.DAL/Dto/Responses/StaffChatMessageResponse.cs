using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class StaffChatMessageResponse
    {
        public int Id { get; set; }
        public int ChatId { get; set; }

        // who sent it
        public string SenderId { get; set; }
        public string SenderName { get; set; }

        // "Patient" or "Staff" — helps frontend align message left/right
        public string SenderRole { get; set; }

        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}
