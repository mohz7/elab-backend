using eLab.BLL.Services.Common;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Interface
{
    public interface ITestCatalogService
    {
        public Task<ServiceResult<List<TestCatalogResponse>>> GetAllAsync(bool onlyActive = false);
        public Task<ServiceResult<TestCatalogResponse>> GetByIdAsync(int id);
        public Task<ServiceResult<string>> CreateAsync(TestCatalogRequest request, string adminId);
        public Task<ServiceResult<string>> UpdateAsync(int id,TestCatalogRequest request);
        public Task<ServiceResult<string>> DeactivateTestCatalogAsync(int id);
        public Task<ServiceResult<string>> ActivateTestCatalogAsync(int id);

    }
}
