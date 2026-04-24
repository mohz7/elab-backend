using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Classes;
using eLab.DAL.Repository.Interface;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Midicare_eLab.DAL.Models;
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
        private readonly UserManager<User> _userManager;
        private readonly IPatientProfileRepository _patientProfileRepository;

        public BookingService(IBookingRepository bookingRepository,
            UserManager<User> userManager,
            IPatientProfileRepository patientProfileRepository)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _patientProfileRepository = patientProfileRepository;
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

        public async Task<ServiceResult<List<Booking>>> GetBookingByPatientAsync(string patientId)
        {
            string identityNumber;

            if (patientId.Length == 9)
            {
                identityNumber = patientId;
            }
            else
            {
                var user = await _userManager.FindByIdAsync(patientId);
                if (user == null)
                    return ServiceResult<List<Booking>>.Fail(404, "User not found", "...");

                identityNumber = user.IdentityNumber;
            }

            var patient = await _patientProfileRepository.GetByIdAsync(identityNumber);
            if (patient == null)
                return ServiceResult<List<Booking>>.Fail(404, "Patient not found", "...");

            var result = await _bookingRepository.GetBookingByPatientAsync(identityNumber);
            return ServiceResult<List<Booking>>.Ok(result);
        }

        public async Task<ServiceResult<PatientProfileResponse?>> GetUserByBookingAsync(int bookingId)
        {
            var patient = await _bookingRepository.GetPatientByBookingAsync(bookingId);
            var result = new PatientProfileResponse()
            {
                Id = patient.Id,
                UserId = patient.UserId,
                Allergies = patient.Allergies,
                BloodType = patient.BloodType,
                ChronicDiseases = patient.ChronicDiseases,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactPhone = patient.EmergencyContactPhone,
                InsuranceCompany = patient.InsuranceCompany,
                InsuranceNumber = patient.InsuranceNumber,
                DateOfBirth = patient.User.DateOfBirth,
                BranchId = patient.BranchId,
                Email = patient.User.Email,
                FullName = patient.User.FullName,
                Gender = patient.User.Gender,
                IdentityNumber = patient.User.IdentityNumber,
                PhoneNumber = patient.User.PhoneNumber,
                Notes = patient.Notes,
            };
            return ServiceResult<PatientProfileResponse>.Ok(result);
        }

        public async Task<ServiceResult<List<Booking>>> GetAll(int? branchId)
        {
            var bookings = await _bookingRepository.GetAllAsync();
            if(branchId.HasValue)
                bookings = bookings.Where(bo => bo.BranchId == branchId).ToList();

            return ServiceResult<List<Booking>>.Ok(bookings);
        }
    }
}
