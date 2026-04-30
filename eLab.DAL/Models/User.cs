using eLab.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Midicare_eLab.DAL.Models
{
    public enum Gender
    {
        Male = 1,
        Female = 2,
    }
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        [MaxLength(9)]
        public string IdentityNumber { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CodeResetPassword { get; set; }
        public DateTime? PasswordResetCodeExpiry { get; set; }

        public int Age => CalculateAge(DateOfBirth);

        private static int CalculateAge(DateOnly dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > DateOnly.FromDateTime(today.AddYears(-age))) age--;
            return age;
        }


        // Reverse Navigation

        // Branch
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();

            // StaffProfile
            public StaffProfile StaffProfile { get; set; }
            [JsonIgnore]
            public ICollection<StaffProfile> StaffProfiles { get; set; } = new List<StaffProfile>();

            // PatientProfile
            [JsonIgnore]
            public PatientProfile PatientProfile { get; set; }
            [JsonIgnore]
            public ICollection<PatientProfile> CreatedPatientProfiles { get; set; } = new List<PatientProfile>();

            // TestCatalog
            public ICollection<TestCatalog> TestCatalogs { get; set; } = new List<TestCatalog>();

            // Price
            public ICollection<Price> Prices { get; set; } = new List<Price>();

            // Offer
            public ICollection<Offer> Offers { get; set; } = new List<Offer>();

            // Booking
            [JsonIgnore]
            public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

            // Result
            public ICollection<Result> UploadedResults { get; set; } = new List<Result>();
            public ICollection<Result> ApprovedResults { get; set; } = new List<Result>();

            // ReferenceRange
            public ICollection<ReferenceRange> ReferenceRanges { get; set; } = new List<ReferenceRange>();
            
            // StaffChatMessage
            public ICollection<StaffChatMessage> StaffChatMessages { get; set; } = new List<StaffChatMessage>();

            // AIChatMessage
            public ICollection<AIChatMessage> AIChatMessages { get; set; } = new List<AIChatMessage>();

            // Notification
            [JsonIgnore]
            public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

            // Cart
            [JsonIgnore]
            public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
