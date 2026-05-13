using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.DTO.Requests
{
    public class LoginRequest
    {
        [EmailAddress]
        [MaxLength(254)]
        public string? Email { get; set; }
        public string? IdentityNumber { get; set; }
        public string Password { get; set; }
    }
}
