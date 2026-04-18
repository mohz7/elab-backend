using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class StaffChatSessionResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookingId { get; set; }
        public string PatientProfileId { get; set; }
        public string StaffProfileId { get; set; }
        public List<StaffChatMessageResponse> StaffChatMessages { get; set; } = new();
    }
}
