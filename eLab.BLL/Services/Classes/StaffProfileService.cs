using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.DTO.Requests;
using eLab.DAL.Repository.Interface;
using Mapster;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midicare_eLab.DAL.Models;
using eLab.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using eLab.DAL.Repository.Classes;

namespace eLab.BLL.Services.Classes
{
    public class StaffProfileService : IStaffProfileService
    {
        private readonly IStaffProfileRepository _staffProfileRepository;
        private readonly UserManager<User> _userManager;

        public StaffProfileService(IStaffProfileRepository staffProfileRepository,
            UserManager<User> userManager)
        {
            _staffProfileRepository = staffProfileRepository;
            _userManager = userManager;
        }

        public async Task<ServiceResult<string>> ChangePasswordAsync(string id, ChangePasswordRequest request)
        {
            if (id.Length != 9)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return ServiceResult<string>.Fail(404, "Patient not found", "...");

                var passwordCheck = await _userManager.CheckPasswordAsync(user, request.OldPassword);
                if (!passwordCheck)
                    return ServiceResult<string>.Fail(403, "Old password is incorrect", "...");

                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            }
            else
            {
                var staff = await _staffProfileRepository.GetByIdAsync(id);
                if (staff is null)
                    return ServiceResult<string>.Fail(404, "Staff not found", "...");

                var passwordCheck = await _userManager.CheckPasswordAsync(staff.User, request.OldPassword);
                if (!passwordCheck)
                    return ServiceResult<string>.Fail(403, "Old password is incorrect", "...");

                var result = await _userManager.ChangePasswordAsync(staff.User, request.OldPassword, request.NewPassword);
            }

            return ServiceResult<string>.Ok("Change passowrd successfully");
        }

        public async Task<ServiceResult<string>> CreateAsync(StaffProfileRequest request, string adminId)
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

            var resultUser = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "Staff");
            if (!resultUser.Succeeded)
                return ServiceResult<string>.Fail(403, "Failed to create user", "...");

            if (!Enum.TryParse<JobTitle>(request.JobTitle, out var jobTitle))
                return ServiceResult<string>.Fail(400, "Invalid JobTitle value", "...");

            var staff = new StaffProfile()
            {
                Id = request.IdentityNumber,
                JobTitle = jobTitle,
                HiredAt = DateTime.UtcNow,
                BranchId = request.BranchId,
                CreatedById = adminId,
                UserId = user.Id
            };
            var resultStaff = await _staffProfileRepository.CreateAsync(staff);
            if(resultStaff < 1)
                return ServiceResult<string>.Fail(403, "Failed to create staff", "...");
            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<List<StaffProfilesResponse>>> GetAllAsync(int? branchId, JobTitle? job, bool? IsActive)
        {
            var staffs = await _staffProfileRepository.GetAllAsync();
            if (!staffs.Any())
                return ServiceResult<List<StaffProfilesResponse>>.Fail(404, "Not any staff", "...");

            if (branchId.HasValue)
                staffs = staffs.Where(st => st.BranchId == branchId).ToList();

            if (job.HasValue)
                staffs = staffs.Where(st => st.JobTitle == job).ToList();

            if (IsActive.HasValue == true)
                staffs = staffs.Where(st => st.User.IsActive == true).ToList();

            var result = staffs.Select(st => new StaffProfilesResponse
            {
                Id = st.Id,
                UserId = st.UserId,
                FullName = st.User?.FullName,
                IdentityNumber = st.User?.IdentityNumber,
                Gender = st.User?.Gender ?? default,
                DateOfBirth = st.User?.DateOfBirth ?? default,
                JobTitle = st.JobTitle,
                HiredAt = st.HiredAt,
                CreatedBy = st.CreatedBy?.FullName,
                BranchName = st.Branch?.Name,
                IsActive = st.User.IsActive
            }).ToList();
            return ServiceResult<List<StaffProfilesResponse>>.Ok(result);

        }

        public async Task<ServiceResult<StaffProfilesResponse>> GetByIdAsync(string id)
        {
            var staff = await _staffProfileRepository.GetByIdAsync(id);
            if (staff is null)
                return ServiceResult<StaffProfilesResponse>.Fail(404, "Staff not found", "...");
            var result = new StaffProfilesResponse
            {
                Id = staff.Id,
                UserId = staff.UserId,
                FullName = staff.User?.FullName,
                IdentityNumber = staff.User?.IdentityNumber,
                Gender = staff.User?.Gender ?? default,
                DateOfBirth = staff.User?.DateOfBirth ?? default,
                JobTitle = staff.JobTitle,
                HiredAt = staff.HiredAt,
                CreatedBy = staff.CreatedBy?.FullName,
                BranchName = staff.Branch?.Name,
                IsActive = staff.User.IsActive
            };
            return ServiceResult<StaffProfilesResponse>.Ok(result);
        }

        public async Task<ServiceResult<string>> RemoveAsync(string id)
        {
            var staff = await _staffProfileRepository.GetByIdAsync(id);
            if (staff is null)
                return ServiceResult<string>.Fail(404, "Staff not found", "...");

            staff.User.IsActive = false;
            var result = await _staffProfileRepository.UpdateAsync(staff);
            if (result < 1)
                return ServiceResult<string>.Fail(500, "Failed to Remove staff", "...");

            return ServiceResult<string>.Ok("Remove successfully");
        }

        public async Task<ServiceResult<string>> UpdateAsync(string id, StaffProfileRequest request)
        {
            var staff = await _staffProfileRepository.GetByIdAsync(id);
            if (staff is null)
                return ServiceResult<string>.Fail(404, "Staff not found", "...");

            if (request.JobTitle != null)
            {
                if (!Enum.TryParse<JobTitle>(request.JobTitle, out var jobTitle))
                    return ServiceResult<string>.Fail(400, "Invalid JobTitle value", "...");

                staff.JobTitle = jobTitle;
            }
            staff.User.IdentityNumber = request.IdentityNumber != null ? request.IdentityNumber : staff.User.IdentityNumber;
            staff.User.FullName = request.FullName != null ? request.FullName : staff.User.FullName;
            staff.User.Email = request.Email != null ? request.Email : staff.User.Email;
            staff.User.Gender = request.Gender != null ? request.Gender : staff.User.Gender;
            staff.User.DateOfBirth = request.DateOfBirth != null ? request.DateOfBirth : staff.User.DateOfBirth;
            staff.User.PhoneNumber = request.PhoneNumber != null ? request.PhoneNumber : staff.User.PhoneNumber;
            staff.User.UserName = request.UserName != null ? request.UserName : staff.User.UserName;

            var result = await _staffProfileRepository.UpdateAsync(staff);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to update staff", "...");

            return ServiceResult<string>.Ok("Update successfully");
        }
    }
}
