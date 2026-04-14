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
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }

        public string Branch { get; set; }
        public string? StaffProfile { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<BookingItemResponse> Items { get; set; }

    }
}
