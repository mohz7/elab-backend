using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class ResultSummaryResponse
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public DateTime ResultDate { get; set; }
        public ResultStatus Status { get; set; }
        public int AbnormalCount { get; set; }
        public bool HasAbnormalValues { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
