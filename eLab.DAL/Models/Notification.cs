using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public enum NotificationType
    {
        ResultReady = 1,
        AppointmentReminder = 2,
        MessageReceived = 3,
        SystemAlert = 4
    }
    public class Notification
    {
        public int Id { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties

            // User
            public string UserId { get; set; }
            public User User { get; set; }

            // Result
            public int? ResultId { get; set; }
            public Result Result { get; set; }
    }
}
