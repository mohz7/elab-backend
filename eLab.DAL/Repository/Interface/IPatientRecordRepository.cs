using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IPatientRecordRepository
    {
        Task<int> CreateAsync(PatientRecord patientRecord);
        Task<List<PatientRecord>> GetAllAsync();
        Task<PatientRecord> GetByIdAsync(int id);
        Task<List<PatientRecord>> GetByPatientProfileIdAsync(string patientProfileId);
        Task<int> RemoveAsync(PatientRecord patientRecord);
        Task<int> UpdateAsync(PatientRecord patientRecord);
    }
}
