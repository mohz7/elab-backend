using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IBranchRepository
    {
        public Task<List<Branch>> GettAllAsync();
        public Task<List<Branch>> GetAllPatientAsync();
        public Task<Branch> GetByIdAsync(int id);
        public Task<int> CreateAsync(Branch branch);
        public Task<int> UpdateAsync(Branch branch);
        //public Task<int> RemoveAsync(Branch branch);
    }
}
