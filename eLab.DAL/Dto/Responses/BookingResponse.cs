using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly BookingTime { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public string Branch { get; set; }
        public string? StaffProfile { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BranchId { get; set; }
        public string PatientProfileId { get; set; }
        public List<BookingItemResponse> BookingItems { get; set; } = new();

    }
}
