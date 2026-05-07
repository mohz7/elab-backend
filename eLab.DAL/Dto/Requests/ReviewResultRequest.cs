using eLab.DAL.Models;
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
        [EnumDataType(typeof(ResultStatus),
            ErrorMessage = "Action must be 'Approved' or 'Rejected'.")]
        public ResultStatus Action { get; set; }

        public string? RejectionNotes { get; set; }
    }
}
