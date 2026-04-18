using eLab.BLL.Services.Common;
using eLab.BLL.Services.Interface;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using eLab.DAL.Repository.Interface;
using Mapster;
using Stripe;
using System.Text;

namespace eLab.BLL.Services.Classes
{
    public class AIChatService : IAIChatService
    {
        private readonly IAIChatRepository _aiChatRepository;
        private readonly IResultRepository _resultRepository;
        //private readonly IAIService _aiService;

        public AIChatService(
            IAIChatRepository aiChatRepository,
            IResultRepository resultRepository)
            //IAIService aiService)
        {
            _aiChatRepository = aiChatRepository;
            _resultRepository = resultRepository;
            //_aiService = aiService;
        }

        public async Task<ServiceResult<AIChatSessionResponse>> StartSessionAsync(int resultId, string patientId)
        {
            var result = await _resultRepository.GetByIdAsync(resultId);
            if (result is null)
                return ServiceResult<AIChatSessionResponse>.Fail(404, "Result not found.", "...");

            if (result.PatientProfileId != patientId)
                return ServiceResult<AIChatSessionResponse>.Fail(403, "This result does not belong to you.", "...");

            var contextSummary = BuildContextSummary(result);

            var session = new AIChat
            {
                PatientProfileId = patientId,
                ResultId = resultId,
                ContextSummary = contextSummary,
                StartedAt = DateTime.UtcNow
            };

            var created = await _aiChatRepository.AddSessionAsync(session);
            if (created != 1)
                return ServiceResult<AIChatSessionResponse>.Fail(400, "Failed to create AI chat session.", "...");

            return ServiceResult<AIChatSessionResponse>.Ok(session.Adapt<AIChatSessionResponse>());
        }

        public async Task<ServiceResult<AIChatSessionResponse>> GetSessionAsync(int aiChatId, string patientId)
        {
            var session = await _aiChatRepository.GetByIdAsync(aiChatId);
            if (session is null)
                return ServiceResult<AIChatSessionResponse>.Fail(404, "AI chat session not found.", "...");

            if (session.PatientProfileId != patientId)
                return ServiceResult<AIChatSessionResponse>.Fail(403, "Access denied.", "...");

            return ServiceResult<AIChatSessionResponse>.Ok(session.Adapt<AIChatSessionResponse>());
        }

        public async Task<ServiceResult<List<AIChatSessionResponse>>> GetMySessionsAsync(string patientId)
        {
            var sessions = await _aiChatRepository.GetByPatientIdAsync(patientId);
            return ServiceResult<List<AIChatSessionResponse>>.Ok(sessions.Adapt<List<AIChatSessionResponse>>());
        }

        public async Task<ServiceResult<AIConversationResponse>> SendMessageAsync(int aiChatId, string patientId, string userMessage)
        {
            var session = await _aiChatRepository.GetByIdAsync(aiChatId);
            if (session is null)
                return ServiceResult<AIConversationResponse>.Fail(404, "AI chat session not found.", "...");

            if (session.PatientProfileId != patientId)
                return ServiceResult<AIConversationResponse>.Fail(403, "Access denied.", "...");

            // 1. save user message
            var userMsg = new AIChatMessage
            {
                AIChatId = aiChatId,
                SenderId = patientId,
                Role = ChatMessageRole.User,
                Message = userMessage,
                SentAt = DateTime.UtcNow
            };

            var userMsgResult = await _aiChatRepository.AddMessageAsync(userMsg);
            if (userMsgResult != 1)
                return ServiceResult<AIConversationResponse>.Fail(400, "Failed to save user message.", "...");

            // 2. build conversation history for context
            var history = await _aiChatRepository.GetMessagesAsync(aiChatId);

            // 3. call AI with result context + full history
            //var aiReply = await _aiService.GetConversationResponseAsync(
            //    session.ContextSummary,
            //    history,
            //    userMessage
            //);

            // 4. save AI response
            var aiMsg = new AIChatMessage
            {
                AIChatId = aiChatId,
                SenderId = null,
                Role = ChatMessageRole.Assistant,
                //Message = aiReply,
                SentAt = DateTime.UtcNow
            };

            var aiMsgResult = await _aiChatRepository.AddMessageAsync(aiMsg);
            if (aiMsgResult != 1)
                return ServiceResult<AIConversationResponse>.Fail(400, "Failed to save AI response.", "...");

            var response = new AIConversationResponse
            {
                UserMessage = userMsg.Adapt<AIChatMessageResponse>(),
                AIResponse = aiMsg.Adapt<AIChatMessageResponse>()
            };

            return ServiceResult<AIConversationResponse>.Ok(response);
        }

        // ── builds the system context injected into every AI prompt ──
        private string BuildContextSummary(Result result)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a helpful medical lab assistant.");
            sb.AppendLine("Explain results in simple, clear language a non-medical person can understand.");
            sb.AppendLine("Never diagnose. Always recommend consulting a doctor for medical decisions.");
            sb.AppendLine();
            sb.AppendLine("Patient Lab Results:");
            sb.AppendLine(System.Text.Json.JsonSerializer.Serialize(result.ResultData));
            return sb.ToString();
        }
    }
}