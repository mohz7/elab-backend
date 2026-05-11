using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Classes;
using eLab.DAL.Repository.Interface;
using Mapster;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class PriceService : IPriceService
    {
        private readonly IPriceRepository _priceRepository;

        public PriceService(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public async Task<ServiceResult<string>> CreateAsync(PriceRequest request, string adminId)
        {
            var price = request.Adapt<Price>();
            price.CreatedById = adminId;
            var result = await _priceRepository.CreateAsync(price);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Created failed", "...");
            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<string>> RemoveAsync(int id)
        {
            var price = await _priceRepository.GetByIdAsync(id);
            if (price is null)
                return ServiceResult<string>.Fail(404, "Price not found", "...");
            await _priceRepository.RemoveAsync(price);
            return ServiceResult<string>.Ok("Deleted successfully");
        }

        public async Task<ServiceResult<List<PriceResponse>>> GetAllAsync(int? branchId)
        {
            var prices = await _priceRepository.GetAllAsync();
            if (branchId.HasValue)
                prices = prices.Where(p => p.BranchId == branchId).ToList();
            if (!prices.Any())
                return ServiceResult<List<PriceResponse>>.Fail(404, "The branch has no Prices", "...");
            var result = prices.Adapt<List<PriceResponse>>();
            return ServiceResult<List<PriceResponse>>.Ok(result);
        }

        public async Task<ServiceResult<PriceResponse>> GetByIdAsync(int id)
        {
            var price = await _priceRepository.GetByIdAsync(id);
            if (price is null)
                return ServiceResult<PriceResponse>.Fail(404, "Price not found", "...");
            var result = price.Adapt<PriceResponse>();
            return ServiceResult<PriceResponse>.Ok(result);
        }

        public async Task<ServiceResult<string>> UpdateAsync(int id, PriceUpdateRequest request)
        {
            var price = await _priceRepository.GetByIdAsync(id);
            if (price is null) return ServiceResult<string>.Fail(404, "Price not found", "...");

            // بدّل هاي السطر ↓
            // request.Adapt(price);

            // وحط هاد بدله ↓
            if (request.BasePrice.HasValue)
                price.BasePrice = request.BasePrice.Value;

            if (request.Currency is not null)
                price.Currency = request.Currency;

            if (request.EffectiveFrom.HasValue)
                price.EffectiveFrom = request.EffectiveFrom.Value;

            if (request.EffectiveTo.HasValue)
                price.EffectiveTo = request.EffectiveTo.Value;

            if (request.TestCatalogId.HasValue)
                price.TestCatalogId = request.TestCatalogId.Value;

            await _priceRepository.UpdateAsync(price);
            return ServiceResult<string>.Ok("Update is successfully");
        }
    }
}
