using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.DTO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IPatientProfileService
    {
        public Task<ServiceResult<string>> CreateAsync(RegisterRequest request, string adminId);
        public Task<ServiceResult<List<PatientProfileResponse>>> GetAllAsync(int? branchId);
        public Task<ServiceResult<PatientProfileResponse>> GetByIdAsync(string id);
        public Task<ServiceResult<PatientProfileResponse>> GetByPatientAsync(string id);
        public Task<ServiceResult<string>> UpdateAsync(string id, RegisterRequest request);
        public Task<ServiceResult<string>> ChangePasswordAsync(string id, ChangePasswordRequest request);
        public Task<ServiceResult<List<PatientProfileResponse>>> GetAllAsync(string staffId);

    }
}
