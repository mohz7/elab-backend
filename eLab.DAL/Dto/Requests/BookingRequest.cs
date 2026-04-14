using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class BookingRequest
    {
        public DateTime BookingDate { get; set; }
        public int BranchId { get; set; }
        public string? StaffProfileId{ get; set; }
        public string? Notes { get; set; }
    }
}
