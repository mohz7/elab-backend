using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Midicare_eLab.DAL.Models;

namespace eLab.BLL.Services.Classes
{
    public class StaffChatService : IStaffChatService
    {
        private readonly IStaffChatRepository _staffChatRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly UserManager<User> _userManager;

        public StaffChatService(
            IStaffChatRepository staffChatRepository,
            IBookingRepository bookingRepository,
            UserManager<User> userManager)
        {
            _staffChatRepository = staffChatRepository;
            _bookingRepository = bookingRepository;
            _userManager = userManager;
        }

        // 🔹 Create Chat Session
        public async Task<ServiceResult<StaffChatSessionResponse>> CreateSessionAsync(int bookingId, string patientId)
        {
            var user = await _userManager.FindByIdAsync(patientId);
            if (await _staffChatRepository.SessionExistsForBookingAsync(bookingId))
                return ServiceResult<StaffChatSessionResponse>.Fail(400, "A chat session already exists for this booking.", "...");

            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Booking not found.", "...");

            if (booking.PatientProfileId != user.IdentityNumber)
                return ServiceResult<StaffChatSessionResponse>.Fail(403, "You are not allowed to create this session.", "...");

            var chat = new StaffChat
            {
                BookingId = bookingId,
                PatientProfileId = patientId,
                StaffProfileId = booking.StaffProfileId,
                CreatedAt = DateTime.UtcNow
            };

            var success = await _staffChatRepository.AddSessionAsync(chat);
            if (success < 0)
                return ServiceResult<StaffChatSessionResponse>.Fail(400, "Failed to create chat session.", "...");

            return ServiceResult<StaffChatSessionResponse>.Ok(MapSession(chat));
        }

        public async Task<ServiceResult<StaffChatSessionResponse>> GetSessionAsync(int chatId, string requestingUserId)
        {
            var chat = await _staffChatRepository.GetByIdAsync(chatId);
            if (chat is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Chat session not found.", "...");

            if (!IsParticipant(chat, requestingUserId))
                return ServiceResult<StaffChatSessionResponse>.Fail(403, "Access denied.", "...");

            return ServiceResult<StaffChatSessionResponse>.Ok(MapSession(chat));
        }

        public async Task<ServiceResult<List<StaffChatSessionResponse>>> GetByPatientIdAsync(string patientId)
        {
            var sessions = await _staffChatRepository.GetByPatientIdAsync(patientId);
            return ServiceResult<List<StaffChatSessionResponse>>.Ok(
                sessions.Select(MapSession).ToList()
            );
        }

        public async Task<ServiceResult<List<StaffChatSessionResponse>>> GetByStaffIdAsync(string staffId)
        {
            var sessions = await _staffChatRepository.GetByStaffIdAsync(staffId);
            return ServiceResult<List<StaffChatSessionResponse>>.Ok(
                sessions.Select(MapSession).ToList()
            );
        }

        public async Task<ServiceResult<StaffChatMessageResponse>> SendMessageAsync(int chatId, string senderId, string message)
        {
            var chat = await _staffChatRepository.GetByIdAsync(chatId);
            if (chat is null)
                return ServiceResult<StaffChatMessageResponse>.Fail(404, "Chat session not found.", "...");

            if (!IsParticipant(chat, senderId))
                return ServiceResult<StaffChatMessageResponse>.Fail(403, "You are not a participant in this chat.", "...");

            if (string.IsNullOrWhiteSpace(message))
                return ServiceResult<StaffChatMessageResponse>.Fail(400, "Message cannot be empty.", "...");

            if (message.Length > 1000)
                return ServiceResult<StaffChatMessageResponse>.Fail(400, "Message too long.", "...");

            var msg = new StaffChatMessage
            {
                ChatId = chatId,
                SenderId = senderId,
                Message = message,
                IsRead = false,
                SentAt = DateTime.UtcNow
            };

            var success = await _staffChatRepository.AddMessageAsync(msg);
            if (success < 0)
                return ServiceResult<StaffChatMessageResponse>.Fail(400, "Failed to send message.", "...");

            return ServiceResult<StaffChatMessageResponse>.Ok(
                MapMessage(msg, chat)
            );
        }

        public async Task<ServiceResult<string>> MarkAsReadAsync(int chatId, string readerId)
        {
            var chat = await _staffChatRepository.GetByIdAsync(chatId);
            if (chat is null)
                return ServiceResult<string>.Fail(404, "Chat session not found.", "...");

            if (!IsParticipant(chat, readerId))
                return ServiceResult<string>.Fail(403, "Access denied.", "...");

            await _staffChatRepository.MarkMessagesAsReadAsync(chatId, readerId);

            return ServiceResult<string>.Ok("Messages marked as read successfully.");
        }

        private bool IsParticipant(StaffChat chat, string userId)
        {
            return chat.PatientProfile?.UserId == userId
                || chat.StaffProfile?.UserId == userId;
        }
        private StaffChatSessionResponse MapSession(StaffChat chat)
        {
            var messages = chat.StaffChatMessages?
                .OrderBy(m => m.SentAt)
                .ToList() ?? new List<StaffChatMessage>();

            return new StaffChatSessionResponse
            {
                ChatId = chat.Id,
                BookingId = chat.BookingId,

                PatientId = chat.PatientProfileId,
                PatientName = chat.PatientProfile?.User.FullName ?? "",

                StaffId = chat.StaffProfileId,
                StaffName = chat.StaffProfile?.User.FullName ?? "",

                CreatedAt = chat.CreatedAt,

                Messages = messages.Select(m => new StaffChatMessageResponse
                {
                    Id = m.Id,
                    ChatId = m.ChatId ?? 0,
                    SenderId = m.SenderId,
                    Message = m.Message,
                    IsRead = m.IsRead,
                    SentAt = m.SentAt
                }).ToList(),

                TotalMessages = messages.Count,

                UnreadCount = messages.Count(m => m.IsRead == false)
            };
        }
        private StaffChatMessageResponse MapMessage(StaffChatMessage msg, StaffChat chat)
        {
            string senderRole =
                chat.PatientProfileId == msg.SenderId ? "Patient" :
                chat.StaffProfileId == msg.SenderId ? "Staff" :
                "Unknown";

            string senderName =
                msg.Sender?.FullName ?? "Unknown";

            return new StaffChatMessageResponse
            {
                Id = msg.Id,
                ChatId = msg.ChatId ?? 0,

                SenderId = msg.SenderId ?? "",
                SenderName = senderName,
                SenderRole = senderRole,

                Message = msg.Message,
                IsRead = msg.IsRead,
                SentAt = msg.SentAt
            };
        }
    }
}