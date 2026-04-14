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
    public interface INotificationService
    {
        Task<ServiceResult<List<NotificationResponse>>> GetAllByUserAsync(string userId);
        Task<ServiceResult<List<NotificationResponse>>> GetAllAsync();
        Task<ServiceResult<NotificationResponse>> GetByIdAsync(int id, string userId);
        Task<ServiceResult<int>> GetUnreadCountAsync(string userId);
        Task<ServiceResult<string>> CreateAsync(NotificationRequest request);
        Task<ServiceResult<string>> MarkAsReadAsync(int id, string userId);
        Task<ServiceResult<string>> MarkAllAsReadAsync(string userId);
        Task<ServiceResult<string>> RemoveAsync(int id, string userId);
    }
}
