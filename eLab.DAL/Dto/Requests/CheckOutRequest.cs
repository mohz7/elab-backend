using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class CheckOutRequest
    {
        public PaymentMethodEnum PaymentMethod { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly BookingTime { get; set; }
        public string Notes { get; set; }
        public int BranchId { get; set; }
    }
}
