using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midicare_eLab.DAL.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties

            // User
            public string CreatedById { get; set; }
            public User CreatedBy { get; set; }


        // Reverse Navigation

            // StaffProfile
            public ICollection<StaffProfile> StaffProfiles { get; set; } = new List<StaffProfile>();

            // Price
            public ICollection<Price> Prices { get; set; } = new List<Price>();

            // Offer
            public ICollection<Offer> Offers { get; set; } = new List<Offer>();

            // Booking
            public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

            // PatientRecord
            public ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();

            //PatientProfile
            public ICollection<PatientProfile> patientProfiles { get; set; } = new List<PatientProfile>();

    }
}
