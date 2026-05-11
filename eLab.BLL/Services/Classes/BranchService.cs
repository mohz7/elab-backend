using Azure.Core;
using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Mapster;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<ServiceResult<string>> CreateAsync(BranchRequest request, string userId)
        {
            var branch = request.Adapt<Branch>();
            branch.CreatedById = userId;
            var result = await _branchRepository.CreateAsync(branch);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Created failed", "...");
            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<List<BranchResponse>>> GetAllAsync()
        {
            var branchs = await _branchRepository.GettAllAsync();
            return ServiceResult<List<BranchResponse>>
            .Ok(branchs.Adapt<List<BranchResponse>>());
        }

        public async Task<ServiceResult<List<BranchPatientResponse>>> GetAllPatientAsync()
        {
            var branchs = await _branchRepository.GetAllPatientAsync();
            return ServiceResult<List<BranchPatientResponse>>
            .Ok(branchs.Adapt<List<BranchPatientResponse>>());
        }

        public async Task<ServiceResult<BranchResponse>> GetByIdAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch is null) 
                return ServiceResult<BranchResponse>.Fail(404, "Branch not found", "...");

            return ServiceResult<BranchResponse>
            .Ok(branch.Adapt<BranchResponse>());
        }

        public async Task<ServiceResult<string>> UpdateAsync(int id, BranchRequest request)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch is null) return ServiceResult<string>.Fail(404,"Branch not found","...");

            request.Adapt(branch);
            var result = await _branchRepository.UpdateAsync(branch);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Update failed", "...");

            return ServiceResult<string>.Ok("Update is successfully");
        }

        public async Task<ServiceResult<string>> DeactivateAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch is null) return ServiceResult<string>.Fail(404, "Branch not found", "...");

            if (!branch.IsActive)
                return ServiceResult<string>.Fail(400, "Branch is already deactivated", "...");

            branch.IsActive = false;
            var result = await _branchRepository.UpdateAsync(branch);
            if (result < 1)
                return ServiceResult<string>.Fail(400, "The change to deactivate failed", "...");
            return ServiceResult<string>.Ok("The change to deactivate successfully");
        }

        public async Task<ServiceResult<string>> ActivateAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch is null) return ServiceResult<string>.Fail(404, "Branch not found", "...");

            if (branch.IsActive)
                return ServiceResult<string>.Fail(400, "Branch is already activated", "...");

            branch.IsActive = true;
            var result = await _branchRepository.UpdateAsync(branch);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "The change to activate failed", "...");
            return ServiceResult<string>.Ok("The change to activate successfully");
        }
    }
}
