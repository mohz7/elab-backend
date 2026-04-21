using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class StaffChatMessageRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "Message cannot be empty.")]
        [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
        public string Message { get; set; }
    }
}
