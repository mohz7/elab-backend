using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Requests
{
    public class StartAISessionRequest
    {
        [Required]
        public int ResultId { get; set; }
    }
}
