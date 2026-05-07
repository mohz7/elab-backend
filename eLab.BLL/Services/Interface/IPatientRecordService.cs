using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IPatientRecordService
    {
        Task<ServiceResult<List<PatientRecordResponse>>> GetAllAsync();
        Task<ServiceResult<PatientRecordResponse>> GetByIdAsync(int id);
        Task<ServiceResult<PatientRecordResponse>> GetByPatientProfileIdAsync(string patientProfileId);
        Task<ServiceResult<string>> CreateAsync(PatientRecordRequest request);
        Task<ServiceResult<string>> RemoveAsync(int id);
        Task<ServiceResult<string>> UpdateAsync(int id, PatientRecordRequest request);
    }
}
