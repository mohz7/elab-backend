using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IStaffProfileRepository
    {
        public Task<List<StaffProfile>> GetAllAsync();
        public Task<int> CreateAsync(StaffProfile staffProfile);
        public Task<StaffProfile> GetByIdAsync(string id);
        public Task<int> UpdateAsync(StaffProfile staffProfile);
    }
}
