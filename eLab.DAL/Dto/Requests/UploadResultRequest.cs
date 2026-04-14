using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class UploadResultRequest
    {
        [Required]
        public int BookingItemId { get; set; }

        [Required]
        public int ReportTemplateId { get; set; }

        [Required]
        public DateTime ResultDate { get; set; }

        [Required]
        public Dictionary<string, decimal> ResultData { get; set; }

        public string? FileUrl { get; set; }
    }
}
