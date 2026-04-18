using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Mapster;

namespace eLab.BLL.Services.Classes
{
    public class StaffChatService : IStaffChatService
    {
        private readonly IStaffChatRepository _staffChatRepository;
        private readonly IBookingRepository _bookingRepository;

        public StaffChatService(
            IStaffChatRepository staffChatRepository,
            IBookingRepository bookingRepository)
        {
            _staffChatRepository = staffChatRepository;
            _bookingRepository = bookingRepository;
        }

        // 🔹 Create Chat Session
        public async Task<ServiceResult<StaffChatSessionResponse>> CreateSessionAsync(int bookingId, string patientId)
        {
            if (await _staffChatRepository.SessionExistsForBookingAsync(bookingId))
                return ServiceResult<StaffChatSessionResponse>.Fail(400, "A chat session already exists for this booking.", "...");

            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Booking not found.", "...");

            // ✅ Authorization: تأكد أن المريض هو صاحب الحجز
            if (booking.PatientProfileId != patientId)
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

            return ServiceResult<StaffChatSessionResponse>.Ok(chat.Adapt<StaffChatSessionResponse>());
        }

        // 🔹 Get Chat Session
        public async Task<ServiceResult<StaffChatSessionResponse>> GetSessionAsync(int chatId, string requestingUserId)
        {
            var chat = await _staffChatRepository.GetByIdAsync(chatId);
            if (chat is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Chat session not found.", "...");

            if (!IsParticipant(chat, requestingUserId))
                return ServiceResult<StaffChatSessionResponse>.Fail(403, "Access denied.", "...");

            return ServiceResult<StaffChatSessionResponse>.Ok(chat.Adapt<StaffChatSessionResponse>());
        }

        // 🔹 Get Patient Sessions
        public async Task<ServiceResult<List<StaffChatSessionResponse>>> GetByPatientIdAsync(string patientId)
        {
            var sessions = await _staffChatRepository.GetByPatientIdAsync(patientId);
            return ServiceResult<List<StaffChatSessionResponse>>.Ok(
                sessions.Adapt<List<StaffChatSessionResponse>>());
        }

        // 🔹 Get Staff Sessions
        public async Task<ServiceResult<List<StaffChatSessionResponse>>> GetByStaffIdAsync(string staffId)
        {
            var sessions = await _staffChatRepository.GetByStaffIdAsync(staffId);
            return ServiceResult<List<StaffChatSessionResponse>>.Ok(
                sessions.Adapt<List<StaffChatSessionResponse>>());
        }

        // 🔹 Send Message
        public async Task<ServiceResult<StaffChatMessageResponse>> SendMessageAsync(int chatId, string senderId, string message)
        {
            var chat = await _staffChatRepository.GetByIdAsync(chatId);
            if (chat is null)
                return ServiceResult<StaffChatMessageResponse>.Fail(404, "Chat session not found.", "...");

            if (!IsParticipant(chat, senderId))
                return ServiceResult<StaffChatMessageResponse>.Fail(403, "You are not a participant in this chat.", "...");

            // ✅ Validation
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

            return ServiceResult<StaffChatMessageResponse>.Ok(msg.Adapt<StaffChatMessageResponse>());
        }

        // 🔹 Mark Messages As Read
        public async Task<ServiceResult<string>> MarkAsReadAsync(int chatId, string readerId)
        {
            var chat = await _staffChatRepository.GetByIdAsync(chatId);
            if (chat is null)
                return ServiceResult<string>.Fail(404, "Chat session not found.", "...");

            // ✅ Authorization
            if (!IsParticipant(chat, readerId))
                return ServiceResult<string>.Fail(403, "Access denied.", "...");

            await _staffChatRepository.MarkMessagesAsReadAsync(chatId, readerId);

            return ServiceResult<string>.Ok("Messages marked as read successfully.");
        }

        // 🔒 Helper Method
        private bool IsParticipant(StaffChat chat, string userId)
        {
            return chat.PatientProfile?.UserId == userId
                || chat.StaffProfile?.UserId == userId;
        }
    }
}