using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Migrations;
using eLab.DAL.Models;
using eLab.DAL.Repository.Classes;
using eLab.DAL.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class CheckOutService : ICheckOutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<User> _userManager;
        private readonly IBranchRepository _branchRepository;
        private readonly IPatientProfileRepository _patientProfileRepository;
        private readonly ITestCatalogRepository _testCatalogRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailSender _emailSender;
        private readonly IBookingItemRepository _bookingItemRepository;
        private readonly IStaffProfileRepository _staffProfileRepository;

        public CheckOutService(ICartRepository cartRepository,
            UserManager<User> userManager,
            IBranchRepository branchRepository,
            IPatientProfileRepository patientProfileRepository,
            ITestCatalogRepository testCatalogRepository,
            IPriceRepository priceRepository,
            IOfferRepository offerRepository,
            IBookingRepository bookingRepository,
            IEmailSender emailSender,
            IBookingItemRepository bookingItemRepository,
            IStaffProfileRepository staffProfileRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _branchRepository = branchRepository;
            _patientProfileRepository = patientProfileRepository;
            _testCatalogRepository = testCatalogRepository;
            _priceRepository = priceRepository;
            _offerRepository = offerRepository;
            _bookingRepository = bookingRepository;
            _emailSender = emailSender;
            _bookingItemRepository = bookingItemRepository;
            _staffProfileRepository = staffProfileRepository;
        }

        public async Task<ServiceResult<bool>> HandlePaymentSuccessAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            var userId = booking.PatientProfile.User.Id;
            var today = DateTime.Today;
            var prices = await _priceRepository.GetAllAsync();
            var offers = await _offerRepository.GetAllAsync();
            var carts = await _cartRepository.GetUserCartAsync(userId);

            // ✅ خزن BookingItems للـ Visa والـ Cash
            var bookingItems = new List<BookingItem>();
            foreach (var cartItem in carts)
            {
                var bestPrice = prices
                    .Where(p => p.TestCatalogId == cartItem.TestCatalogId
                        && p.BranchId == booking.BranchId
                        && p.EffectiveFrom <= today
                        && (p.EffectiveTo == null || p.EffectiveTo >= today))
                    .OrderByDescending(p => p.EffectiveFrom)
                    .FirstOrDefault();

                var bestOffer = offers
                    .Where(o => o.TestCatalogId == cartItem.TestCatalogId &&
                        o.BranchId == booking.BranchId &&
                        o.IsActive &&
                        o.StartDate <= today &&
                        o.EndDate >= today)
                    .OrderByDescending(o => o.DiscountPercent)
                    .FirstOrDefault();

                decimal discount = bestOffer != null
                    ? bestPrice.BasePrice * (bestOffer.DiscountPercent / 100m)
                    : 0;

                var finalUnitPrice = Math.Max(0, bestPrice.BasePrice - discount);

                bookingItems.Add(new BookingItem
                {
                    BookingId = bookingId,
                    TestCatalogId = cartItem.TestCatalogId,
                    FinalPrice = finalUnitPrice * cartItem.CountItems,
                    UnitPrice = bestPrice.BasePrice,
                });
            }

            await _bookingItemRepository.AddRangeAsync(bookingItems);
            await _cartRepository.ClearCartAsync(userId);

            string subject, body;

            if (booking.PaymentMethod == PaymentMethodEnum.Visa)
            {
                booking.Status = Status.Confirmed;
                subject = "Payment successful - eLab";
                body = $"<h1>Thank you for your payment</h1>" +
                       $"<p>Booking ID: {bookingId}</p>" +
                       $"<p>Total Amount: {booking.TotalAmount}</p>";
            }
            else
            {
                subject = "Booking Placed successfully - eLab";
                body = $"<h1>Thank you for your booking</h1>" +
                       $"<p>Booking ID: {bookingId}</p>" +
                       $"<p>Total Amount: {booking.TotalAmount}</p>";
            }

            await _emailSender.SendEmailAsync(booking.PatientProfile.User.Email, subject, body);
            return ServiceResult<bool>.Ok(true);
        }

        public async Task<ServiceResult<CheckOutResponse>> ProcessPaymentAsync(CheckOutRequest request, string userId, HttpRequest Request)
        {
            var result = new CheckOutResponse();
            const int SLOT_DURATION = 20;

            // Check user
            var user = await _userManager.FindByIdAsync(userId);
            var patient = await _patientProfileRepository.GetByIdAsync(user.IdentityNumber);
            if (user is null || !user.IsActive)
                return ServiceResult<CheckOutResponse>.Fail(404, "User not found or inactive", "...");
            if (patient is null)
                return ServiceResult<CheckOutResponse>.Fail(404, "Patient profile not found", "...");

            // Check Branch
            var branch = await _branchRepository.GetByIdAsync(request.BranchId);
            if (branch is null || !branch.IsActive)
                return ServiceResult<CheckOutResponse>.Fail(404, "Branch not found or inactive", "...");

            // Check test basket
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
                return ServiceResult<CheckOutResponse>.Fail(404, "Test basket is empty", "...");

            // Check Tests
            var testIds = cartItems.Select(se => se.TestCatalogId).Distinct().ToList();

            var activeTestIds = (await _testCatalogRepository.GetAllAsync())
                .Where(t => t.IsActive)
                .Select(t => t.Id)
                .ToList();

            var allValid = testIds.All(id => activeTestIds.Contains(id));
            if (!allValid)
                return ServiceResult<CheckOutResponse>.Fail(403, "One or more tests are inactive or not found", "...");

            // Check Prices
            var today = DateTime.Today;
            var prices = await _priceRepository.GetAllAsync();

            var validPrices = prices
                .Where(p => testIds.Contains(p.TestCatalogId)
                    && p.BranchId == request.BranchId
                    && p.EffectiveFrom <= today
                    && (p.EffectiveTo == null || p.EffectiveTo >= today))
                .GroupBy(p => p.TestCatalogId)
                .Select(g => g.OrderByDescending(p => p.EffectiveFrom).First())
                .ToList();

            if (validPrices.Count != testIds.Count)
                return ServiceResult<CheckOutResponse>.Fail(403, "Pricing not available for one or more tests in this branch", "...");

            // Check Offers (optional)
            var offers = await _offerRepository.GetAllAsync();
            var validOffers = offers
                .Where(o =>
                    testIds.Contains(o.TestCatalogId) &&
                    o.BranchId == request.BranchId &&
                    o.IsActive &&
                    o.StartDate <= today &&
                    o.EndDate >= today)
                .ToList();

            decimal totalAmount = 0;
            foreach (var price in validPrices)
            {
                var offer = validOffers
                    .Where(o => o.TestCatalogId == price.TestCatalogId)
                    .OrderByDescending(o => o.DiscountPercent)
                    .FirstOrDefault();

                decimal discount = offer != null
                    ? price.BasePrice * (offer.DiscountPercent / 100m)
                    : 0;

                var cartItem = cartItems.FirstOrDefault(c => c.TestCatalogId == price.TestCatalogId);
                int quantity = cartItem?.CountItems ?? 1;

                totalAmount += Math.Max(0, price.BasePrice - discount) * quantity;
            }

            // Check Date
            var todayDate = DateOnly.FromDateTime(DateTime.Today);
            if (request.BookingDate < todayDate)
                return ServiceResult<CheckOutResponse>.Fail(403, "Booking date cannot be in the past", "...");

            // Check Time Slot Conflict
            var start = request.BookingTime;
            var end = start.AddMinutes(SLOT_DURATION);

            var allBookings = await _bookingRepository.GetAllAsync();
            var hasConflict = allBookings.Any(b =>
                b.BranchId == request.BranchId &&
                b.BookingDate == request.BookingDate &&
                start < b.BookingTime.AddMinutes(SLOT_DURATION) &&
                end > b.BookingTime);

            if (hasConflict)
                return ServiceResult<CheckOutResponse>.Fail(403, "This time slot is already booked", "...");

            // Create Booking
            var booking = new Booking
            {
                PatientProfileId = patient.Id,
                PaymentMethod = request.PaymentMethod,
                TotalAmount = totalAmount,
                BranchId = request.BranchId,
                BookingDate = request.BookingDate,
                BookingTime = request.BookingTime,
                Notes = request.Notes
            };

            await _bookingRepository.AddAsync(booking);

            if (request.PaymentMethod == PaymentMethodEnum.Cash)
            {
                await HandlePaymentSuccessAsync(booking.Id);

                return ServiceResult<CheckOutResponse>.Ok(new CheckOutResponse
                {
                    Success = true,
                    Message = "cash"
                });
            }

            if (request.PaymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/api/Customer/CheckOuts/success/{booking.Id}",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
                };

                foreach (var item in cartItems)
                {
                    var itemPrice = validPrices
                        .FirstOrDefault(p => p.TestCatalogId == item.TestCatalogId);

                    var itemOffer = validOffers
                        .Where(o => o.TestCatalogId == item.TestCatalogId)
                        .OrderByDescending(o => o.DiscountPercent)
                        .FirstOrDefault();

                    decimal itemDiscount = itemOffer != null
                        ? itemPrice.BasePrice * (itemOffer.DiscountPercent / 100m)
                        : 0;

                    long finalUnitAmount = (long)Math.Max(0, itemPrice.BasePrice - itemDiscount);

                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.TestCatalog.Name,
                                Description = item.TestCatalog.Description,
                            },
                            UnitAmount = finalUnitAmount,
                        },
                        Quantity = item.CountItems,
                    });
                }

                var service = new SessionService();
                var session = service.Create(options);

                booking.PaymentId = session.Id;

                result = new CheckOutResponse
                {
                    Success = true,
                    Message = "Payment session created successfully",
                    PaymentId = session.Id,
                    Url = session.Url,
                };
                return ServiceResult<CheckOutResponse>.Ok(result);
            }

            result = new CheckOutResponse
            {
                Success = true,
                Message = "cash"
            };
            return ServiceResult<CheckOutResponse>.Ok(result);
        }

        public async Task<ServiceResult<CheckOutResponse>> ProcessPaymentByStaffAsync(string patientId, CheckOutRequest request, string userId, HttpRequest Request)
        {
            var result = new CheckOutResponse();
            const int SLOT_DURATION = 20;

            // Check Staff
            var staff = await _userManager.FindByIdAsync(userId);
            if (staff is null || !staff.IsActive)
                return ServiceResult<CheckOutResponse>.Fail(404, "Staff not found or inactive", "...");

            // Check Patient
            var patient = await _patientProfileRepository.GetByIdAsync(patientId);
            if (patient is null)
                return ServiceResult<CheckOutResponse>.Fail(404, "Patient profile not found", "...");

            var patientUser = await _userManager.FindByIdAsync(patient.UserId);
            if (patientUser is null || !patientUser.IsActive)
                return ServiceResult<CheckOutResponse>.Fail(404, "Patient not found or inactive", "...");

            // Check Branch
            var branch = await _branchRepository.GetByIdAsync(request.BranchId);
            if (branch is null || !branch.IsActive)
                return ServiceResult<CheckOutResponse>.Fail(404, "Branch not found or inactive", "...");

            // Staff can only create cash bookings
            if (request.PaymentMethod != PaymentMethodEnum.Cash)
                return ServiceResult<CheckOutResponse>.Fail(400, "Staff can only create cash bookings", "...");

            // Check Cart
            var cartItems = await _cartRepository.GetUserCartAsync(patientId);
            if (!cartItems.Any())
                return ServiceResult<CheckOutResponse>.Fail(404, "Test basket is empty", "...");

            // Check Tests
            var testIds = cartItems.Select(se => se.TestCatalogId).Distinct().ToList();

            var activeTestIds = (await _testCatalogRepository.GetAllAsync())
                .Where(t => t.IsActive)
                .Select(t => t.Id)
                .ToList();

            var allValid = testIds.All(id => activeTestIds.Contains(id));
            if (!allValid)
                return ServiceResult<CheckOutResponse>.Fail(403, "One or more tests are inactive or not found", "...");

            // Check Prices
            var today = DateTime.Today;
            var prices = await _priceRepository.GetAllAsync();

            var validPrices = prices
                .Where(p => testIds.Contains(p.TestCatalogId)
                    && p.BranchId == request.BranchId
                    && p.EffectiveFrom <= today
                    && (p.EffectiveTo == null || p.EffectiveTo >= today))
                .GroupBy(p => p.TestCatalogId)
                .Select(g => g.OrderByDescending(p => p.EffectiveFrom).First())
                .ToList();

            if (validPrices.Count != testIds.Count)
                return ServiceResult<CheckOutResponse>.Fail(403, "Pricing not available for one or more tests in this branch", "...");

            // Check Offers (optional)
            var offers = await _offerRepository.GetAllAsync();
            var validOffers = offers
                .Where(o =>
                    testIds.Contains(o.TestCatalogId) &&
                    o.BranchId == request.BranchId &&
                    o.IsActive &&
                    o.StartDate <= today &&
                    o.EndDate >= today)
                .ToList();

            // ✅ حساب الـ TotalAmount مع الـ discount
            decimal totalAmount = 0;
            foreach (var price in validPrices)
            {
                var offer = validOffers
                    .Where(o => o.TestCatalogId == price.TestCatalogId)
                    .OrderByDescending(o => o.DiscountPercent)
                    .FirstOrDefault();

                decimal discount = offer != null
                    ? price.BasePrice * (offer.DiscountPercent / 100m)
                    : 0;

                var cartItem = cartItems.FirstOrDefault(c => c.TestCatalogId == price.TestCatalogId);
                int quantity = cartItem?.CountItems ?? 1;

                totalAmount += Math.Max(0, price.BasePrice - discount) * quantity;
            }

            // Check Date
            var todayDate = DateOnly.FromDateTime(DateTime.Today);
            if (request.BookingDate < todayDate)
                return ServiceResult<CheckOutResponse>.Fail(403, "Booking date cannot be in the past", "...");

            // Check Time Slot Conflict
            var start = request.BookingTime;
            var end = start.AddMinutes(SLOT_DURATION);

            var allBookings = await _bookingRepository.GetAllAsync();
            var hasConflict = allBookings.Any(b =>
                b.BranchId == request.BranchId &&
                b.BookingDate == request.BookingDate &&
                start < b.BookingTime.AddMinutes(SLOT_DURATION) &&
                end > b.BookingTime);

            if (hasConflict)
                return ServiceResult<CheckOutResponse>.Fail(403, "This time slot is already booked", "...");

            // Create Booking
            var booking = new Booking
            {
                PatientProfileId = patient.Id,
                PaymentMethod = PaymentMethodEnum.Cash,
                TotalAmount = totalAmount, // ✅ بيحسب مع الـ discount
                BranchId = request.BranchId,
                BookingDate = request.BookingDate,
                BookingTime = request.BookingTime,
                Notes = request.Notes,
                CreatedById = userId,
                StaffProfileId = staff.IdentityNumber
            };

            await _bookingRepository.AddAsync(booking);

            // Create Booking Items
            var bookingItems = new List<BookingItem>();
            foreach (var cartItem in cartItems)
            {
                var bestPrice = validPrices
                    .FirstOrDefault(p => p.TestCatalogId == cartItem.TestCatalogId);

                var bestOffer = validOffers
                    .Where(o => o.TestCatalogId == cartItem.TestCatalogId)
                    .OrderByDescending(o => o.DiscountPercent)
                    .FirstOrDefault();

                decimal discount = bestOffer != null
                    ? bestPrice.BasePrice * (bestOffer.DiscountPercent / 100m)
                    : 0;

                var finalUnitPrice = Math.Max(0, bestPrice.BasePrice - discount);

                bookingItems.Add(new BookingItem
                {
                    BookingId = booking.Id,
                    TestCatalogId = cartItem.TestCatalogId,
                    UnitPrice = bestPrice.BasePrice,
                    FinalPrice = finalUnitPrice * cartItem.CountItems,
                });
            }

            await _bookingItemRepository.AddRangeAsync(bookingItems);
            await _cartRepository.ClearCartAsync(patientId);

            // Send Email to Patient
            await _emailSender.SendEmailAsync(
                patientUser.Email,
                "Booking Placed by Staff - eLab",
                $"<h1>Your booking has been placed by our staff</h1>" +
                $"<p>Booking ID: {booking.Id}</p>" +
                $"<p>Total Amount: {booking.TotalAmount}</p>"
            );

            return ServiceResult<CheckOutResponse>.Ok(new CheckOutResponse
            {
                Success = true,
                Message = "Booking created successfully by staff"
            });
        }
    }
}