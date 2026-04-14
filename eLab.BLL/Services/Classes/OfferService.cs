using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Classes;
using eLab.DAL.Repository.Interface;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<ServiceResult<string>> ActivateTestCatalogAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);

            if (offer == null)
                return ServiceResult<string>.Fail(404, "Offer not found", "...");

            if (offer.IsActive)
                return ServiceResult<string>.Fail(400, "Already active", "...");

            offer.IsActive = true;

            await _offerRepository.UpdateAsync(offer);

            return ServiceResult<string>.Ok("Activated successfully");
        }

        public async Task<ServiceResult<string>> CreateAsync(OfferRequest request, string adminId)
        {
            var offer = request.Adapt<Offer>();
            offer.CreatedById = adminId;
            var result = await _offerRepository.CreateAsync(offer);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Created failed", "...");
            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<string>> DeactivateTestCatalogAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);

            if (offer == null)
                return ServiceResult<string>.Fail(404, "Offer not found", "...");

            if (!offer.IsActive)
                return ServiceResult<string>.Fail(400, "Already inactive", "...");

            offer.IsActive = false;

            await _offerRepository.UpdateAsync(offer);

            return ServiceResult<string>.Ok("The offer was successfully deactivated");
        }

        public async Task<ServiceResult<List<OfferResponse>>> GetAllAsync(int? branchId, bool onlyActive = false)
        {
            var offers = await _offerRepository.GetAllAsync();
            if (onlyActive)
                offers = offers.Where(o => o.IsActive == true).ToList();
            if (branchId.HasValue)
                offers = offers.Where(o => o.BranchId == branchId).ToList();
            if (!offers.Any())
                return ServiceResult<List<OfferResponse>>.Fail(404, "The branch has no offers", "...");

            var result = offers.Adapt<List<OfferResponse>>();
            return ServiceResult<List<OfferResponse>>.Ok(result);
        }

        public async Task<ServiceResult<OfferResponse>> GetByIdAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer is null)
                return ServiceResult<OfferResponse>.Fail(404, "Offer not found", "...");
            var result = offer.Adapt<OfferResponse>();
            return ServiceResult<OfferResponse>.Ok(result);
        }

        public async Task<ServiceResult<string>> UpdateAsync(int id, OfferRequest request)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer is null) return ServiceResult<string>.Fail(404, "Offer not found", "...");
            request.Adapt(offer);
            var result = await _offerRepository.UpdateAsync(offer);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Update failed", "...");
            return ServiceResult<string>.Ok("Update is successfully");
        }
    }
}
