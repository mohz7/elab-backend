using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Midicare_eLab.DAL.Models;

namespace eLab.BLL.Services.Classes
{
    public class StaffChatService : IStaffChatService
    {
        private readonly IStaffChatRepository _staffChatRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly UserManager<User> _userManager;
        private readonly IResultRepository _resultRepository;
        private readonly IPatientProfileRepository _patientProfileRepository;
        private readonly IStaffProfileRepository _staffProfileRepository;

        public StaffChatService(
            IStaffChatRepository staffChatRepository,
            IBookingRepository bookingRepository,
            UserManager<User> userManager,
            IResultRepository resultRepository,
            IPatientProfileRepository patientProfileRepository,
            IStaffProfileRepository staffProfileRepository)
        {
            _staffChatRepository = staffChatRepository;
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _resultRepository = resultRepository;
            _patientProfileRepository = patientProfileRepository;
            _staffProfileRepository = staffProfileRepository;
        }

        public async Task<ServiceResult<StaffChatSessionResponse>> CreateSessionAsync(int resultId, string patientId)
        {
            // 1. تحقق من المستخدم
            var user = await _userManager.FindByIdAsync(patientId);
            if (user is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "User not found.", "...");

            // 2. تحقق إن ما في session موجودة
            if (await _staffChatRepository.SessionExistsForResultAsync(resultId))
                return ServiceResult<StaffChatSessionResponse>.Fail(400, "A chat session already exists for this result.", "...");

            // 3. جيب الـ Result
            var result = await _resultRepository.GetByIdAsync(resultId);
            if (result is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Result not found.", "...");

            // 4. جيب الـ Booking
            var booking = await _bookingRepository.GetByResultIdAsync(resultId);
            if (booking is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Booking not found for this result.", "...");

            // 5. جيب PatientProfile عن طريق Repository
            var patientProfile = await _userManager.FindByIdAsync(patientId);
            if (patientProfile is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Patient profile not found.", "...");

            // 6. تأكد إن المريض هو صاحب الحجز
            if (booking.PatientProfileId != patientProfile.IdentityNumber)
                return ServiceResult<StaffChatSessionResponse>.Fail(403, "You are not allowed to create this session.", "...");

            // 7. جيب StaffProfile عن طريق Repository
            var staffProfile = await _userManager.FindByIdAsync(result.UploadedById);
            if (staffProfile is null)
                return ServiceResult<StaffChatSessionResponse>.Fail(404, "Staff profile not found.", "...");

            // 8. أنشئ الـ session بالـ IDs الصحيحة
            var chat = new StaffChat
            {
                BookingId = booking.Id,
                ResultId = resultId,
                PatientProfileId = patientProfile.IdentityNumber,
                StaffProfileId = staffProfile.IdentityNumber,
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
            var patientProfile = await _userManager.FindByIdAsync(patientId);
            if (patientProfile is null)
                return ServiceResult<List<StaffChatSessionResponse>>.Fail(404, "Patient profile not found.", "...");

            var sessions = await _staffChatRepository.GetByPatientIdAsync(patientProfile.IdentityNumber);
            return ServiceResult<List<StaffChatSessionResponse>>.Ok(
                sessions.Select(MapSession).ToList()
            );
        }

        public async Task<ServiceResult<List<StaffChatSessionResponse>>> GetByStaffIdAsync(string staffId)
        {
            var staffProfile = await _userManager.FindByIdAsync(staffId);
            if (staffProfile is null)
                return ServiceResult<List<StaffChatSessionResponse>>.Fail(404, "Staff profile not found.", "...");

            var sessions = await _staffChatRepository.GetByStaffIdAsync(staffProfile.IdentityNumber);
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

            return ServiceResult<StaffChatMessageResponse>.Ok(await MapMessage(msg, chat));
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

        // ─── Helpers ────────────────────────────────────────────────

        private bool IsParticipant(StaffChat chat, string userId)
            => chat.PatientProfile?.UserId == userId
            || chat.StaffProfile?.UserId == userId;

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
                PatientName = chat.PatientProfile?.User?.FullName ?? "",

                StaffId = chat.StaffProfileId,
                StaffName = chat.StaffProfile?.User?.FullName ?? "",

                CreatedAt = chat.CreatedAt,

                Messages = messages.Select(m => new StaffChatMessageResponse
                {
                    Id = m.Id,
                    ChatId = m.ChatId ?? 0,
                    SenderId = m.SenderId ?? "",
                    SenderName = m.Sender?.FullName ?? "Unknown",
                    SenderRole = chat.PatientProfile?.UserId == m.SenderId ? "Patient"
                               : chat.StaffProfile?.UserId == m.SenderId ? "Staff"
                               : "Unknown",
                    Message = m.Message,
                    IsRead = m.IsRead,
                    SentAt = m.SentAt
                }).ToList(),

                TotalMessages = messages.Count,
                UnreadCount = messages.Count(m => !m.IsRead)
            };
        }

        private async Task<StaffChatMessageResponse> MapMessage(StaffChatMessage msg, StaffChat chat)
        {
            var fullChat = await _staffChatRepository.GetByIdAsync(chat.Id);

            string senderRole = fullChat?.PatientProfile?.UserId == msg.SenderId ? "Patient"
                              : fullChat?.StaffProfile?.UserId == msg.SenderId ? "Staff"
                              : "Unknown";

            var user = await _userManager.FindByIdAsync(msg.SenderId);

            return new StaffChatMessageResponse
            {
                Id = msg.Id,
                ChatId = msg.ChatId ?? 0,
                SenderId = msg.SenderId ?? "",
                SenderName = user?.FullName ?? "Unknown",
                SenderRole = senderRole,
                Message = msg.Message,
                IsRead = msg.IsRead,
                SentAt = msg.SentAt
            };
        }
    }
}