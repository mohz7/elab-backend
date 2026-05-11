using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Migrations;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<User> _userManager;
        private readonly IPatientProfileRepository _patientProfileRepository;

        public CartService(ICartRepository cartRepository,
            UserManager<User> userManager,
            IPatientProfileRepository patientProfileRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _patientProfileRepository = patientProfileRepository;
        }

        public async Task<ServiceResult<string>> AddToCartAsync(CartRequest request, string UserId)
        {
            User user = null;
            if (UserId.Length == 9)
            {
                var patient = await _patientProfileRepository.GetByIdAsync(UserId);
                user = await _userManager.FindByIdAsync(patient.UserId);
            }
            else
            {
                user = await _userManager.FindByIdAsync(UserId);
            }
            if (user is null)
                return ServiceResult<string>.Fail(404, "User not found", "...");

            var newItem = new Cart
            {
                TestCatalogId = request.TestCatalogId,
                UserId = user.Id,
                CountItems = 1
            };
            var result = await _cartRepository.AddAsync(newItem);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to Add item to test basket", "...");

            return ServiceResult<string>.Ok("Add in successfully");
        }

        public async Task<ServiceResult<CartSummaryRespones>> CartSummaryResponesAsync(string UserId)
        {
            User user = null;
            if (UserId.Length == 9)
            {
                var patient = await _patientProfileRepository.GetByIdAsync(UserId);
                user = await _userManager.FindByIdAsync(patient.UserId);
            }
            else
            {
                user = await _userManager.FindByIdAsync(UserId);
            }
            if (user is null)
                return ServiceResult<CartSummaryRespones>.Fail(404, "User not found", "...");

            var cartItems = await _cartRepository.GetUserCartAsync(user.Id);
            if (!cartItems.Any())
                return ServiceResult<CartSummaryRespones>.Fail(404, "Cart is empty", "...");

            var now = DateTime.UtcNow;

            var response = new CartSummaryRespones
            {
                Items = cartItems.Select(ic => new CartResponse
                {
                    Id = ic.Id,
                    TestCatalogId = ic.TestCatalogId,
                    TestCatalogName = ic.TestCatalog?.Name,

                    Price = ic.TestCatalog?.Prices?
                        .Where(p => p.EffectiveFrom <= now && p.EffectiveTo >= now)
                        .Select(p => p.BasePrice)
                        .FirstOrDefault() ?? 0

                }).ToList()
            };

            return ServiceResult<CartSummaryRespones>.Ok(response);
        }

        public async Task<ServiceResult<string>> ClearCartAsync(string userId)
        {
            User user = null;
            if (userId.Length == 9)
            {
                var patient = await _patientProfileRepository.GetByIdAsync(userId);
                user = await _userManager.FindByIdAsync(patient.UserId);
            }
            else
            {
                user = await _userManager.FindByIdAsync(userId);
            }
            var result = await _cartRepository.ClearCartAsync(user.Id);
            if (!result)
                return ServiceResult<string>.Fail(500, "Failed to Clear test basket", "...");

            return ServiceResult<string>.Ok("Clear successfully");
        }

        public async Task<ServiceResult<string>> RemoveOneItemAsync(string userId, int itemId)
        {
            User user = null;
            if (userId.Length == 9)
            {
                var patient = await _patientProfileRepository.GetByIdAsync(userId);
                user = await _userManager.FindByIdAsync(patient.UserId);
            }
            else
            {
                user = await _userManager.FindByIdAsync(userId);
            }
            if (user is null)
                return ServiceResult<string>.Fail(404, "User not found", "...");

            var cartItems = await _cartRepository.GetUserCartAsync(user.Id);
            if (!cartItems.Any())
                return ServiceResult<string>.Fail(404, "Cart is empty", "...");
            var item = await _cartRepository.GetById(itemId);
            if(item is null)
                return ServiceResult<string>.Fail(404, "This item not found", "...");
            var result = await _cartRepository.RemoveOneItemAsync(item);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Remove item is failed", "...");
            return ServiceResult<string>.Ok("Remove item is successfully");

        }
    }
}
