using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking> GetByIdAsync(int bookingId);
        Task<Booking?> GetUserByBookingAsync(int bookingId);
        Task<Booking> AddAsync(Booking booking);
        Task<List<Booking>> GetBookingByUserAsync(string patientId);
        Task<List<Booking>> GetByStatusAsync(Status status);
        Task<bool> ChangeStatusAsync(int bookingId, Status newStatus);
    }
}
