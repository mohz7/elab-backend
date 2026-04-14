using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<ServiceResult<Booking>> AddAsync(Booking booking)
        {
            var result = await _bookingRepository.AddAsync(booking);
            return ServiceResult<Booking>.Ok(result);
        }

        public async Task<ServiceResult<bool>> ChangeStatusAsync(int bookingId, Status newStatus)
        {
            var result = await _bookingRepository.ChangeStatusAsync(bookingId, newStatus);
            return ServiceResult<bool>.Ok(result);
        }

        public async Task<ServiceResult<List<Booking>>> GetByStatusAsync(Status status)
        {
            var result = await _bookingRepository.GetByStatusAsync(status);
            return ServiceResult<List<Booking>>.Ok(result);
        }

        public async Task<ServiceResult<List<Booking>>> GetBookingByUserAsync(string userId)
        {
            var result = await _bookingRepository.GetBookingByUserAsync(userId);
            return ServiceResult<List<Booking>>.Ok(result);
        }

        public async Task<ServiceResult<Booking?>> GetUserByBookingAsync(int bookingId)
        {
            var result = await _bookingRepository.GetUserByBookingAsync(bookingId);
            return ServiceResult<Booking>.Ok(result);
        }
    }
}
