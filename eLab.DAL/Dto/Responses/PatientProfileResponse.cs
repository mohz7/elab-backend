using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Dto.Responses
{
    public class PatientProfileResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string? BloodType { get; set; }
        public string? Allergies { get; set; }
        public string? ChronicDiseases { get; set; }
        public string? Notes { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? InsuranceCompany { get; set; }
        public string? InsuranceNumber { get; set; }
        public int? BranchId { get; set; }
    }
}
