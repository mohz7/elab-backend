using eLab.DAL.Models;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class StaffProfilesResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string IdentityNumber { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public JobTitle JobTitle { get; set; }
        public DateTime HiredAt { get; set; }
        public string CreatedBy { get; set; }
        public string BranchName { get; set; }
        public bool IsActive { get; set; }
    }
}
