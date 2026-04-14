using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class PatientProfile
    {
        public string Id { get; set; }
        public string? BloodType { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }

        // Extra
        public string? Notes { get; set; }

        // Emergency
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }

        // Insurance (optional)
        public string? InsuranceCompany { get; set; }
        public string? InsuranceNumber { get; set; }


        // Navigation properties

        // User
            public string UserId { get; set; }
            public User User { get; set; }
            public string? CreatedById { get; set; }
            public User? CreatedBy { get; set; }

        // Branch
            public int? BranchId { get; set; }
            public Branch? Branch { get; set; }


        // Reverse Navigation

            // Booking
            public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

            // Result
            public ICollection<Result> Results { get; set; } = new List<Result>();

            // PatientRecord
            public ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();

            // StaffChat
            public ICollection<StaffChat> StaffChats { get; set; } = new List<StaffChat>();

            // AIChat
            public ICollection<AIChat> AIChats { get; set; } = new List<AIChat>();

    }
}
