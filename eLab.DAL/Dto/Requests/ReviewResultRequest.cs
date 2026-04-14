using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class ReviewResultRequest
    {
        [Required]
        [RegularExpression("approve|reject",
        ErrorMessage = "Action must be 'approve' or 'reject'.")]
        public string Action { get; set; }

        public string? RejectionNotes { get; set; }
    }
}
