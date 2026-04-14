using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface ITestCatalogRepository
    {
        public Task<List<TestCatalog>> GetAllAsync();
        public Task<TestCatalog> GetByIdAsync(int id);
        public Task<int> CreateAsync(TestCatalog testCatalog);
        public Task<int> UpdateAsync(TestCatalog testCatalog);
    }
}
