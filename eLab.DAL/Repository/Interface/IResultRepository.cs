using eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.DAL.Repository.Interface
{
    public interface IResultRepository
    {
        Task<Result?> GetByIdAsync(int resultId);
        Task<Result?> GetByBookingItemIdAsync(int bookingItemId);
        Task<IEnumerable<Result>> GetByPatientIdAsync(string patientProfileId);
        Task<IEnumerable<Result>> GetPendingApprovalAsync(int branchId);
        Task<bool> ResultExistsForBookingItemAsync(int bookingItemId);
        Task<int> AddAsync(Result result);
        Task<int> UpdateAsync(Result result);
        Task<List<Result>> GetAll();
    }
}
