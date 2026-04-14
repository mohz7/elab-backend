using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IPatientProfileRepository
    {
        public Task<int> CreateAsync(PatientProfile patientProfile);
        public Task<List<PatientProfile>> GetAllAsync();
        public Task<PatientProfile> GetByIdAsync(string id);
        public Task<int> UpdateAsync(PatientProfile patientProfile);
    }
}
