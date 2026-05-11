using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class StaffChat
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }


        // Navigation properties

            // Booking
            public int BookingId { get; set; }
            public Booking? Booking { get; set; }

            // PatientProfile
            public string PatientProfileId { get; set; }
            public PatientProfile PatientProfile { get; set; }

            // StaffProfile
            public string StaffProfileId { get; set; }
            public StaffProfile StaffProfile { get; set; }
           
            // Result
            public int ResultId { get; set; }
            public Result Result { get; set; }


        // Reverse Navigation

            // StaffChatMessage
            public ICollection<StaffChatMessage> StaffChatMessages { get; set; } = new List<StaffChatMessage>();

    }
}
