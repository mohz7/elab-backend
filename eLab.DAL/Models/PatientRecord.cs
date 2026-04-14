using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public class PatientRecord
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }


        // Navigation properties

            // PatientProfile
            public string PatientProfileId { get; set; }
            public PatientProfile PatientProfile { get; set; }

            // Result
            public int ResultId { get; set; }
            public Result Result { get; set; }

            // Booking
            public int BookingId { get; set; }
            public Booking Booking { get; set; }

            // Branch
            public int BranchId { get; set; }
            public Branch Branch { get; set; }

    }
}
