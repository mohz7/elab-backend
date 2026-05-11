using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface ICartRepository
    {
        Task<int> AddAsync(Cart cart);
        Task<List<Cart>> GetUserCartAsync(string UserId);
        Task<bool> ClearCartAsync(string userId);
        Task<Cart> GetById(int itemId);
        Task<int> RemoveOneItemAsync(Cart cart);
    }
}
