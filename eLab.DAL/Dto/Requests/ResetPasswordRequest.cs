using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.DTO.Requests
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string code { get; set; }
        public string NewPassword { get; set; }
    }
}
