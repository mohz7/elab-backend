using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public enum Status
    {
        Pending = 1,
        Confirmed = 2,
        Cancelled = 3,
        Completed = 4
    }
    public enum PaymentStatus
    {
        Unpaid = 1,
        Paid = 2,
        Refunded = 3
    }
    public enum PaymentMethodEnum
    {
        Cash = 1,
        Visa = 2
    }
    public class Booking
    {
        public int Id { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly BookingTime { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? PaymentId { get; set; }


        // Navigation properties

            // PatientProfile
            public string PatientProfileId { get; set; }
            public PatientProfile? PatientProfile { get; set; }

            // Branch
            public int BranchId { get; set; }
            public Branch? Branch { get; set; }

            // StaffProfile
            public string? StaffProfileId { get; set; }
            public StaffProfile? StaffProfile { get; set; }

            // User
            public string? CreatedById { get; set; }
            public User? CreatedBy { get; set; }


        // Reverse Navigation

            // BookingItem
            public ICollection<BookingItem>? BookingItems { get; set; } = new List<BookingItem>();

            // PatientRecord
            public ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();

            // StaffChat
            public ICollection<StaffChat> StaffChats { get; set; } = new List<StaffChat>();
    }
}
