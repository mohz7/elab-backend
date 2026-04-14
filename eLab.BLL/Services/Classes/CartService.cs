using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
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

        public CartService(ICartRepository cartRepository,
            UserManager<User> userManager)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
        }

        public async Task<ServiceResult<string>> AddToCartAsync(CartRequest request, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user is null)
                return ServiceResult<string>.Fail(404, "User not found", "...");

            var newItem = new Cart
            {
                TestCatalogId = request.TestCatalogId,
                UserId = UserId,
                CountItems = 1
            };
            var result = await _cartRepository.AddAsync(newItem);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to Add item to test basket", "...");

            return ServiceResult<string>.Ok("Add in successfully");
        }

        public async Task<ServiceResult<CartSummaryRespones>> CartSummaryResponesAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user is null)
                return ServiceResult<CartSummaryRespones>.Fail(404, "User not found", "...");

            var cartItems = await _cartRepository.GetUserCartAsync(UserId);
            if (!cartItems.Any())
                return ServiceResult<CartSummaryRespones>.Fail(404, "The user's test basket was not found", "...");

            var response = new CartSummaryRespones
            {
                Items = cartItems.Select(ic => new CartResponse
                {
                    TestCatalogId = ic.TestCatalogId,
                    TestCatalogName = ic.TestCatalog.Name,
                    //Price = ic.TestCatalog.Price.BasePrice
                }).ToList()
            };
            return ServiceResult<CartSummaryRespones>.Ok(response);
        }

        public async Task<ServiceResult<string>> ClearCartAsync(string userId)
        {
            var result = await _cartRepository.ClearCartAsync(userId);
            if (!result)
                return ServiceResult<string>.Fail(500, "Failed to Clear test basket", "...");

            return ServiceResult<string>.Ok("Clear successfully");
        }
    }
}
