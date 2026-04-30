using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class ResultResponse
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public string TemplateName { get; set; }
        public DateTime ResultDate { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public ResultStatus Status { get; set; }
        public string? FileUrl { get; set; }
        public string UploadedByName { get; set; }
        public string? ApprovedByName { get; set; }
        public List<ResultParameterResponse> Parameters { get; set; }
            = new List<ResultParameterResponse>();
        public int TotalParameters { get; set; }
        public int AbnormalCount { get; set; }
        public bool HasAbnormalValues => AbnormalCount > 0;
    }
}
