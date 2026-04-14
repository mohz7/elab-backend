using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public enum JobTitle
    {
        Administrator = 1,
        LabManager = 2,
    }
    public class StaffProfile
    {
        public string Id { get; set; }
        public JobTitle JobTitle { get; set; }
        public DateTime HiredAt { get; set; } = DateTime.UtcNow;


        // Navigation properties

            // User
            public string UserId { get; set; }
            public User User { get; set; }
            public string CreatedById { get; set; }
            public User CreatedBy { get; set; }

            // Branch
            public int BranchId { get; set; }
            public Branch Branch { get; set; }


        // Reverse Navigation

            // Booking
            public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

            // StaffChat
            public ICollection<StaffChat> StaffChats { get; set; } = new List<StaffChat>();
    }
}
