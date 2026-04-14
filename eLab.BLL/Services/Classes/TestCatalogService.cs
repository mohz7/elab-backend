using Azure;
using Azure.Core;
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
    public class TestCatalogService : ITestCatalogService
    {
        private readonly ITestCatalogRepository _testCatalogRepository;

        public TestCatalogService(ITestCatalogRepository testCatalogRepository)
        {
            _testCatalogRepository = testCatalogRepository;
        }

        public async Task<ServiceResult<string>> CreateAsync(TestCatalogRequest request, string adminId)
        {
            var testCatalog = request.Adapt<TestCatalog>();
            testCatalog.CreatedById = adminId;
            var result = await _testCatalogRepository.CreateAsync(testCatalog);
            if (result != 1)
                return ServiceResult<string>.Fail(400, "Created failed", "...");
            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<string>> DeactivateTestCatalogAsync(int id)
        {
            var testCatalog = await _testCatalogRepository.GetByIdAsync(id);

            if (testCatalog == null)
                return ServiceResult<string>.Fail(404, "TestCatalog not found", "...");

            if (!testCatalog.IsActive)
                return ServiceResult<string>.Fail(400, "Already inactive", "...");

            if (testCatalog.BookingItems.Any())
                return ServiceResult<string>.Fail(403, "Cannot deactivate, used in bookings", "...");
            
            testCatalog.IsActive = false;

            await _testCatalogRepository.UpdateAsync(testCatalog);

            return ServiceResult<string>.Ok("The test catalog was successfully deactivated");
        }

        public async Task<ServiceResult<string>> ActivateTestCatalogAsync(int id)
        {
            var testCatalog = await _testCatalogRepository.GetByIdAsync(id);

            if (testCatalog == null)
                return ServiceResult<string>.Fail(404, "TestCatalog not found", "...");

            if (testCatalog.IsActive)
                return ServiceResult<string>.Fail(400, "Already active", "...");

            testCatalog.IsActive = true;

            await _testCatalogRepository.UpdateAsync(testCatalog);

            return ServiceResult<string>.Ok("Activated successfully");
        }

        public async Task<ServiceResult<List<TestCatalogResponse>>> GetAllAsync(bool onlyActive = false)
        {
            
            var tests = await _testCatalogRepository.GetAllAsync();
            if (onlyActive)
            {
                tests = tests.Where(c => c.IsActive == true).ToList();
            }
            var result = tests.Adapt<List<TestCatalogResponse>>();
            return ServiceResult<List<TestCatalogResponse>>.Ok(result);
        }

        public async Task<ServiceResult<TestCatalogResponse>> GetByIdAsync(int id)
        {
            var test = await _testCatalogRepository.GetByIdAsync(id);
            if (test is null)
                return ServiceResult<TestCatalogResponse>.Fail(404, "Offer not found", "...");
            var result = test.Adapt<TestCatalogResponse>();
            return ServiceResult<TestCatalogResponse>.Ok(result);
        }

        public async Task<ServiceResult<string>> UpdateAsync(int id, TestCatalogRequest request)
        {
            var testCatalog = await _testCatalogRepository.GetByIdAsync(id);
            if (testCatalog is null) return ServiceResult<string>.Fail(404, "TestCatalog not found", "...");
            request.Adapt(testCatalog);
            var result = await _testCatalogRepository.UpdateAsync(testCatalog);
            return ServiceResult<string>.Ok("Update is successfully");
        }
    }
}
