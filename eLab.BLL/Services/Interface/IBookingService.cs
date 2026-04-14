using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface IBookingService
    {
        Task<ServiceResult<Booking?>> GetUserByBookingAsync(int BookingId);
        Task<ServiceResult<Booking>> AddAsync(Booking booking);
        Task<ServiceResult<List<Booking>>> GetBookingByUserAsync(string userId);
        Task<ServiceResult<List<Booking>>> GetByStatusAsync(Status status);
        Task<ServiceResult<bool>> ChangeStatusAsync(int orderId, Status newStatus);
    }
}
