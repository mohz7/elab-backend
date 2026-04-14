using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Classes
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public OfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Offer offer)
        {
            _context.Offers.Add(offer);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetAllAsync()
        {
            return await _context.Offers.ToListAsync();
        }

        public async Task<Offer> GetByIdAsync(int id)
        {
            return await _context.Offers.FindAsync(id);
        }

        public async Task<int> UpdateAsync(Offer offer)
        {
            _context.Offers.Update(offer);
            return await _context.SaveChangesAsync();
        }
    }
}
