using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IBookingItemRepository
    {
        public Task AddRangeAsync(List<BookingItem> items);
        public Task<BookingItem> GetByIdAsync(int id);
    }
}
