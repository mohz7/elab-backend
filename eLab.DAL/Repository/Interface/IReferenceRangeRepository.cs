using eLab.DAL.Models;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IReferenceRangeRepository
    {
        public Task<List<ReferenceRange>> GetAllAsync();
        public Task<ReferenceRange> GetByIdAsync(int id);
        public Task<int> CreateAsync(ReferenceRange referenceRange);
        public Task<int> RemoveAsync(ReferenceRange referenceRange);
        public Task<int> UpdateAsync(ReferenceRange referenceRange);
        public Task<List<ReferenceRange>> GetByTemplateIdAsync(int reportTemplateId, int age, Gender gender);
    }
}
