using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.DTO.Requests;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midicare_eLab.DAL.Models;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Migrations;
using eLab.DAL.Repository.Classes;
using eLab.DAL.Dto.Requests;
using RegisterRequest = eLab.DAL.DTO.Requests.RegisterRequest;

namespace eLab.BLL.Services.Classes
{
    public class PatientProfileService : IPatientProfileService
    {
        private readonly IPatientProfileRepository _patientProfileRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStaffProfileRepository _staffProfileRepository;

        public PatientProfileService(IPatientProfileRepository patientProfileRepository,
            UserManager<User> userManager,
            IStaffProfileRepository staffProfileRepository)
        {
            _patientProfileRepository = patientProfileRepository;
            _userManager = userManager;
            _staffProfileRepository = staffProfileRepository;
        }

        public async Task<ServiceResult<string>> ChangePasswordAsync(string id, ChangePasswordRequest request)
        {
            PatientProfile? patient = null;
            if (id.Length != 9)
            {
                var user = await _userManager.FindByIdAsync(id);
                patient = await _patientProfileRepository.GetByIdAsync(user.IdentityNumber);

            }
            else
            {
                patient = await _patientProfileRepository.GetByIdAsync(id);
            }
            if (patient is null)
                return ServiceResult<string>.Fail(404, "Patient not found", "...");

            var passwordCheck = await _userManager.CheckPasswordAsync(patient.User, request.OldPassword);
            if (!passwordCheck)
                return ServiceResult<string>.Fail(403, "Old password is incorrect", "...");

            var result = await _userManager.ChangePasswordAsync(patient.User, request.OldPassword, request.NewPassword);
            
            return ServiceResult<string>.Ok("Change passowrd successfully");
        }

        public async Task<ServiceResult<string>> CreateAsync(RegisterRequest request, string adminId)
        {
            var user = new User()
            {
                FullName = request.FullName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                IdentityNumber = request.IdentityNumber,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var patient = new PatientProfile()
                {
                    Id = user.IdentityNumber,
                    UserId = user.Id,
                    BloodType = request.BloodType,
                    ChronicDiseases = request.ChronicDiseases,
                    Allergies = request.Allergies,
                    EmergencyContactName = request.EmergencyContactName,
                    EmergencyContactPhone = request.EmergencyContactPhone,
                    InsuranceCompany = request.InsuranceCompany,
                    InsuranceNumber = request.InsuranceNumber,
                    Notes = request.Notes,
                    CreatedById = adminId,
                    BranchId = request.BranchId
                };
                var resultPatient = await _patientProfileRepository.CreateAsync(patient);
                if (resultPatient != 1)
                    return ServiceResult<string>.Fail(403, "Failed to create Patient", "...");

                await _userManager.AddToRoleAsync(user, "Patient");
                return ServiceResult<string>.Ok("Created successfully");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ServiceResult<string>.Fail(400, "Failed to create user", errors);
            }
        }

        public async Task<ServiceResult<string>> CreateByStaffAsync(RegisterRequest request, string staffId)
        {
            var staffInfo = await _userManager.FindByIdAsync(staffId);
            var staff = await _staffProfileRepository.GetByIdAsync(staffInfo.IdentityNumber);
            var user = new User()
            {
                FullName = request.FullName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                IdentityNumber = request.IdentityNumber,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var patient = new PatientProfile()
                {
                    Id = user.IdentityNumber,
                    UserId = user.Id,
                    BloodType = request.BloodType,
                    ChronicDiseases = request.ChronicDiseases,
                    Allergies = request.Allergies,
                    EmergencyContactName = request.EmergencyContactName,
                    EmergencyContactPhone = request.EmergencyContactPhone,
                    InsuranceCompany = request.InsuranceCompany,
                    InsuranceNumber = request.InsuranceNumber,
                    Notes = request.Notes,
                    CreatedById = staffId,
                    BranchId = staff.BranchId
                };
                var resultPatient = await _patientProfileRepository.CreateAsync(patient);
                if (resultPatient != 1)
                    return ServiceResult<string>.Fail(403, "Failed to create Patient", "...");

                await _userManager.AddToRoleAsync(user, "Patient");
                return ServiceResult<string>.Ok("Created successfully");
            }
            else
            {
                throw new Exception($"{result.Errors}");
            }
        }

        public async Task<ServiceResult<List<PatientProfileResponse>>> GetAllAsync(int? branchId)
        {
            var patients = await _patientProfileRepository.GetAllAsync();
            if (!patients.Any())
                return ServiceResult<List<PatientProfileResponse>>.Fail(404, "Patients not found", "...");

            if (branchId.HasValue)
                patients = patients.Where(pa => pa.BranchId == branchId).ToList();

            var result = patients.Select(st => new PatientProfileResponse
            {
                Id = st.Id,
                UserId = st.UserId,
                FullName = st.User?.FullName,
                IdentityNumber = st.User?.IdentityNumber,
                Gender = st.User?.Gender ?? default,
                DateOfBirth = st.User?.DateOfBirth ?? default,
                BranchId = st.BranchId,
                Allergies = st.Allergies,
                BloodType = st.BloodType,
                ChronicDiseases = st.ChronicDiseases,
                Email = st.User.Email,
                EmergencyContactName = st.EmergencyContactName,
                EmergencyContactPhone = st.EmergencyContactPhone,
                InsuranceCompany = st.InsuranceCompany,
                InsuranceNumber = st.InsuranceNumber,
                Notes = st.Notes,
                PhoneNumber = st.User.PhoneNumber
            }).ToList();

            return ServiceResult<List<PatientProfileResponse>>.Ok(result);
        }

        public async Task<ServiceResult<List<PatientProfileResponse>>> GetAllAsync(string staffId)
        {
            var patients = await _patientProfileRepository.GetAllAsync();

            if (!patients.Any())
                return ServiceResult<List<PatientProfileResponse>>.Fail(404, "Patients not found", "...");
            var user = await _userManager.FindByIdAsync(staffId);
            var staff = await _staffProfileRepository.GetByIdAsync(user.IdentityNumber);

            if (staff is null)
                return ServiceResult<List<PatientProfileResponse>>.Fail(404, "Staff not found", "...");

            patients = patients
                .Where(pa => pa.BranchId.HasValue && pa.BranchId == staff.BranchId)
                .ToList();

            var result = patients.Select(st => new PatientProfileResponse
            {
                Id = st.Id,
                UserId = st.UserId,
                FullName = st.User?.FullName,
                IdentityNumber = st.User?.IdentityNumber,
                Gender = st.User?.Gender ?? default,
                DateOfBirth = st.User?.DateOfBirth ?? default,
                BranchId = st.BranchId,
                Allergies = st.Allergies,
                BloodType = st.BloodType,
                ChronicDiseases = st.ChronicDiseases,
                Email = st.User?.Email,             
                EmergencyContactName = st.EmergencyContactName,
                EmergencyContactPhone = st.EmergencyContactPhone,
                InsuranceCompany = st.InsuranceCompany,
                InsuranceNumber = st.InsuranceNumber,
                Notes = st.Notes,
                PhoneNumber = st.User?.PhoneNumber   
            }).ToList();

            return ServiceResult<List<PatientProfileResponse>>.Ok(result);
        }

        public async Task<ServiceResult<PatientProfileResponse>> GetByIdAsync(string id)
        {
            PatientProfile? patient = null;
            if (id.Length != 9)
            {
                var user = await _userManager.FindByIdAsync(id);
                patient = await _patientProfileRepository.GetByIdAsync(user.IdentityNumber);

            }
            else
            {
                patient = await _patientProfileRepository.GetByIdAsync(id);
            }
            if (patient is null)
                return ServiceResult<PatientProfileResponse>.Fail(404, "Patient not found", "...");
            var result = new PatientProfileResponse()
            {
                Id = patient.Id,
                UserId = patient.UserId,
                Allergies = patient.Allergies,
                BloodType = patient.BloodType,
                ChronicDiseases = patient.ChronicDiseases,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactPhone = patient.EmergencyContactPhone,
                InsuranceCompany = patient.InsuranceCompany,
                InsuranceNumber = patient.InsuranceNumber,
                DateOfBirth = patient.User.DateOfBirth,
                BranchId = patient.BranchId,
                Email = patient.User.Email,
                FullName = patient.User.FullName,
                Gender = patient.User.Gender,
                IdentityNumber = patient.User.IdentityNumber,
                PhoneNumber = patient.User.PhoneNumber,
                Notes = patient.Notes,
            };
            return ServiceResult<PatientProfileResponse>.Ok(result);
        }
        public async Task<ServiceResult<PatientProfileResponse>> GetByPatientAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return ServiceResult<PatientProfileResponse>.Fail(404, "Patient not found", "...");

            var patient = await _patientProfileRepository.GetByIdAsync(user.IdentityNumber);
            var result = new PatientProfileResponse()
            {
                Id = patient.Id,
                UserId = patient.UserId,
                Allergies = patient.Allergies,
                BloodType = patient.BloodType,
                ChronicDiseases = patient.ChronicDiseases,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactPhone = patient.EmergencyContactPhone,
                InsuranceCompany = patient.InsuranceCompany,
                InsuranceNumber = patient.InsuranceNumber,
                DateOfBirth = patient.User.DateOfBirth,
                BranchId = patient.BranchId,
                Email = patient.User.Email,
                FullName = patient.User.FullName,
                Gender = patient.User.Gender,
                IdentityNumber = patient.User.IdentityNumber,
                PhoneNumber = patient.User.PhoneNumber,
                Notes = patient.Notes,
            };
            return ServiceResult<PatientProfileResponse>.Ok(result);
        }

        public async Task<ServiceResult<string>> UpdateAsync(string id, UpdatePatientRequest request)
        {
            var patient = await _patientProfileRepository.GetByIdAsync(id);
            if (patient is null)
                return ServiceResult<string>.Fail(404, "Patient not found", "...");

            patient.User.IdentityNumber = request.IdentityNumber ?? patient.User.IdentityNumber;
            patient.User.FullName = request.FullName ?? patient.User.FullName;
            patient.User.Email = request.Email ?? patient.User.Email;
            patient.User.Gender = request.Gender ?? patient.User.Gender;
            patient.User.DateOfBirth = request.DateOfBirth ?? patient.User.DateOfBirth;
            patient.User.PhoneNumber = request.PhoneNumber ?? patient.User.PhoneNumber;
            patient.User.UserName = request.UserName ?? patient.User.UserName;
            patient.Allergies = request.Allergies ?? patient.Allergies;
            patient.BloodType = request.BloodType ?? patient.BloodType;
            patient.InsuranceNumber = request.InsuranceNumber ?? patient.InsuranceNumber;
            patient.InsuranceCompany = request.InsuranceCompany ?? patient.InsuranceCompany;
            patient.ChronicDiseases = request.ChronicDiseases ?? patient.ChronicDiseases;
            patient.EmergencyContactPhone = request.EmergencyContactPhone ?? patient.EmergencyContactPhone;
            patient.EmergencyContactName = request.EmergencyContactName ?? patient.EmergencyContactName;
            patient.Notes = request.Notes ?? patient.Notes;

            //if (request.BranchId.HasValue)
            //    patient.BranchId = request.BranchId.Value;

            var result = await _patientProfileRepository.UpdateAsync(patient);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to update patient", "...");

            return ServiceResult<string>.Ok("Update successfully");
        }
    }
}
