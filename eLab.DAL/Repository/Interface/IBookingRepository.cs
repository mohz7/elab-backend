using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using Midicare_eLab.DAL.Models;
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
        Task<PatientProfile?> GetPatientByBookingAsync(int bookingId);
        Task<Booking> AddAsync(Booking booking);
        Task<List<Booking>> GetBookingByPatientAsync(string patientId);
        Task<List<Booking>> GetByStatusAsync(Status status);
        Task<bool> ChangeStatusAsync(int bookingId, Change_statusRequest newStatus);
        Task UpdateAsync(Booking booking);

    }
}
