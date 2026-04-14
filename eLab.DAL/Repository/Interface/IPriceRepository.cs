using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IPriceRepository
    {
        public Task<List<Price>> GetAllAsync();
        public Task<Price> GetByIdAsync(int id);
        public Task<int> CreateAsync(Price price);
        public Task<int> UpdateAsync(Price price);
        public Task<int> RemoveAsync(Price price);
    }
}
