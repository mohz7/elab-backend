using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class PatientRecordItemResponse
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateOnly BookingDate { get; set; }
        public int ResultId { get; set; }
        public string TestName { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
