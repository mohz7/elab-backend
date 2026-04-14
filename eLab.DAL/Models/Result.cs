using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Models
{
    public enum ResultStatus
    {
        Pending,
        Approved,
        Uploaded,
        Reviewed,
        Rejected
    }
    public enum ResultFlags
    {
        Normal = 1,
        High = 2,
        Low = 3
    }
    public class Result
    {
        public int Id { get; set; }
        public DateTime ResultDate { get; set; }
        public ResultStatus Status { get; set; } = ResultStatus.Pending;
        public string ResultData { get; set; }   // the actual test values JSON
        public ResultFlags ResultFlags { get; set; }
        public string FileUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }



        // Navigation properties

            // BookingItem
            public int? BookingItemId { get; set; }
            public BookingItem? BookingItem { get; set; }

            // PatientProfile
            public string? PatientProfileId { get; set; }
            public PatientProfile? PatientProfile { get; set; }

            // ReportTemplate
            public int? ReportTemplateId { get; set; }
            public ReportTemplate? ReportTemplate { get; set; }

            // User
            public string? UploadedById { get; set; }
            public User? UploadedBy { get; set; }
            public string? ApprovedById { get; set; }
            public User? ApprovedBy { get; set; }


        // Reverse Navigation

            // PatientRecord
            public ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();

            // AIChat
            public ICollection<AIChat> AIChats { get; set; } = new List<AIChat>();

            // Notification
            public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
