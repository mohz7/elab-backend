using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class Change_statusRequest
    {
        public Status Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
