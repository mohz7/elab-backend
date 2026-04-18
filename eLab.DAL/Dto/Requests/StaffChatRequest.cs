using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class StaffChatRequest
    {
        public int BookingId { get; set; }
        public string PatientProfileId { get; set; }
    }
}
