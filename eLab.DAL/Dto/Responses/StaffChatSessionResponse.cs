using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class StaffChatSessionResponse
    {
        public int ChatId { get; set; }
        public int BookingId { get; set; }

        // patient info
        public string PatientId { get; set; }
        public string PatientName { get; set; }

        // staff info
        public string StaffId { get; set; }
        public string StaffName { get; set; }

        public DateTime CreatedAt { get; set; }

        // all messages ordered by SentAt
        public List<StaffChatMessageResponse> Messages { get; set; } = new List<StaffChatMessageResponse>();

        // convenience counts for frontend badge/notification
        public int TotalMessages { get; set; }
        public int UnreadCount { get; set; }
    }
}
