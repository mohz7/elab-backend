using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IOfferRepository
    {
        public Task<List<Offer>> GetAllAsync();
        public Task<Offer> GetByIdAsync(int id);
        public Task<int> CreateAsync(Offer offer);
        public Task<int> UpdateAsync(Offer offer);
    }
}
