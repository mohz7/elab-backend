using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLab.BLL.Services.Classes
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<User> _userManager;

        public NotificationService(INotificationRepository notificationRepository,
            UserManager<User> userManager)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }

        public async Task<ServiceResult<List<NotificationResponse>>> GetAllByUserAsync(string userId)
        {
            var notifications = await _notificationRepository.GetAllByUserAsync(userId);
            if (!notifications.Any())
                return ServiceResult<List<NotificationResponse>>.Fail(404, "Not any Notifications", "...");

            var result = notifications.Adapt<List<NotificationResponse>>();
            return ServiceResult<List<NotificationResponse>>.Ok(result);
        }

        public async Task<ServiceResult<List<NotificationResponse>>> GetAllAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            if (!notifications.Any())
                return ServiceResult<List<NotificationResponse>>.Fail(404, "Not any Notifications", "...");

            var result = notifications.Adapt<List<NotificationResponse>>();
            return ServiceResult<List<NotificationResponse>>.Ok(result);
        }

        public async Task<ServiceResult<NotificationResponse>> GetByIdAsync(int id, string userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(id, userId);
            if (notification is null)
                return ServiceResult<NotificationResponse>.Fail(404, "Notification not found", "...");

            var result = notification.Adapt<NotificationResponse>();
            return ServiceResult<NotificationResponse>.Ok(result);
        }

        public async Task<ServiceResult<int>> GetUnreadCountAsync(string userId)
        {
            var result = await _notificationRepository.GetUnreadCountAsync(userId);
            if (result < 1)
                return ServiceResult<int>.Fail(404, "Notification not found", "...");

            return ServiceResult<int>.Ok(result);
        }

        public async Task<ServiceResult<string>> CreateAsync(NotificationRequest request)
        {
            var notification = request.Adapt<Notification>();
            var user = await _userManager.FindByIdAsync(notification.UserId);
            if (user is null)
                return ServiceResult<string>.Fail(404, "User not found", "...");

            var result = await _notificationRepository.CreateAsync(notification);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to create notification", "...");

            return ServiceResult<string>.Ok("Created successfully");
        }

        public async Task<ServiceResult<string>> MarkAsReadAsync(int id, string userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(id, userId);
            if (notification is null)
                return ServiceResult<string>.Fail(404, "Notification not found", "...");

            notification.IsRead = true;
            var result = await _notificationRepository.UpdateAsync(notification);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to change notification status", "...");

            return ServiceResult<string>.Ok("The status has been successfully changed to readable");
        }

        public async Task<ServiceResult<string>> MarkAllAsReadAsync(string userId)
        {
            var result = await _notificationRepository.MarkAllAsReadAsync(userId);
            if (result < 1)
                return ServiceResult<string>.Fail(403, "Failed to change notifications status", "...");

            return ServiceResult<string>.Ok("The notification status change was successfully completed");
        }

        public async Task<ServiceResult<string>> RemoveAsync(int id, string userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(id, userId);
            if (notification is null)
                return ServiceResult<string>.Fail(404, "Notification not found", "...");

            await _notificationRepository.RemoveAsync(notification);
            return ServiceResult<string>.Ok("Deleted successfully");
        }
    }
}
