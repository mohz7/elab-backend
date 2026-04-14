using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IStaffProfileService
    {
        public Task<ServiceResult<List<StaffProfilesResponse>>> GetAllAsync(int? branchId, JobTitle? job, bool? IsActive);
        public Task<ServiceResult<string>> CreateAsync(StaffProfileRequest request, string adminId);
        public Task<ServiceResult<StaffProfilesResponse>> GetByIdAsync(string id);
        public Task<ServiceResult<string>> UpdateAsync(string id, StaffProfileRequest request);
        public Task<ServiceResult<string>> RemoveAsync(string id);
    }
}
