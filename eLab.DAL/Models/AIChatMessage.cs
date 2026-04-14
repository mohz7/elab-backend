using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class AIChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;


        // Navigation properties

            // AIChat
            public int AIChatId { get; set; }
            public AIChat AIChat { get; set; }

            // User
            public string SenderId { get; set; }
            public User? Sender { get; set; }

    }
}
