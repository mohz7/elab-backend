using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class StaffChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }


        // Navigation properties

            // staffChat
            public int? ChatId { get; set; }
            public StaffChat? Chat { get; set; }

            // User
            public string? SenderId { get; set; }
            public User? Sender { get; set; }
    }
}
