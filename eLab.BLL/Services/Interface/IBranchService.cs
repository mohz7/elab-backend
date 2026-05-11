using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IBranchService
    {
        public Task<ServiceResult<List<BranchResponse>>> GetAllAsync();
        public Task<ServiceResult<List<BranchPatientResponse>>> GetAllPatientAsync();
        public Task<ServiceResult<BranchResponse>> GetByIdAsync(int id);
        public Task<ServiceResult<string>> CreateAsync(BranchRequest request, string userId);
        public Task<ServiceResult<string>> UpdateAsync(int id, BranchRequest request);
        public Task<ServiceResult<string>> DeactivateAsync(int id);
        public Task<ServiceResult<string>> ActivateAsync(int id);
    }
}
