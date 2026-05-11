using eLab.BLL.Services.Interface;
using eLab.DAL.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace eLab.BLL.Services.Classes
{
    public class GeminiAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public GeminiAIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            var baseUrl = config["Gemini:BaseUrl"]
                ?? throw new InvalidOperationException("Gemini:BaseUrl is not configured.");

            _httpClient.BaseAddress = new Uri(baseUrl);

            _apiKey = config["Gemini:ApiKey"]
                ?? throw new InvalidOperationException("Gemini API key is not configured.");

            _model = config["Gemini:Model"] ?? "gemini-2.5-flash";
        }

        public async Task<string> GetConversationResponseAsync(
            string contextSummary,
            IEnumerable<AIChatMessage> history,
            string newMessage)
        {
            var contents = new List<object>();

            // Detect language from patient message
            bool isArabic = IsArabic(newMessage);

            var languageInstruction = isArabic
                ? "IMPORTANT: Always reply in Arabic language only."
                : "IMPORTANT: Always reply in English language only.";

            // System context
            contents.Add(new
            {
                role = "user",
                parts = new[]
                {
                    new
                    {
                        text = $"""
                        {contextSummary}

                        {languageInstruction}

                        Additional Rules:
                        - Reply in the SAME language as the patient.
                        - Keep answers short and simple.
                        - Use friendly and easy-to-understand language.
                        - Never give dangerous medical advice.
                        """
                    }
                }
            });

            // AI acknowledgment
            contents.Add(new
            {
                role = "model",
                parts = new[]
                {
                    new
                    {
                        text = isArabic
                            ? "فهمت، سأجيب باللغة العربية بشكل واضح وبسيط."
                            : "Understood. I will reply in clear and simple English."
                    }
                }
            });

            // Conversation history
            foreach (var msg in history.OrderBy(x => x.SentAt).TakeLast(10))
            {
                contents.Add(new
                {
                    role = msg.Role == ChatMessageRole.User ? "user" : "model",
                    parts = new[]
                    {
                        new
                        {
                            text = msg.Message
                        }
                    }
                });
            }

            // New patient message
            contents.Add(new
            {
                role = "user",
                parts = new[]
                {
                    new
                    {
                        text = newMessage
                    }
                }
            });

            var requestBody = new
            {
                contents,
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 1024
                }
            };

            var url =
                $"v1beta/models/{_model}:generateContent?key={_apiKey}";

            HttpResponseMessage response = null!;

            // Retry for 429 errors
            for (int i = 0; i < 3; i++)
            {
                response = await _httpClient.PostAsJsonAsync(url, requestBody);

                if (response.IsSuccessStatusCode)
                    break;

                if ((int)response.StatusCode == 429)
                {
                    await Task.Delay(3000);
                    continue;
                }

                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error ({response.StatusCode}): {error}");
            }

            if (!response.IsSuccessStatusCode)
            {
                return isArabic
                    ? "عذرًا، حدث خطأ أثناء التواصل مع الذكاء الاصطناعي."
                    : "Sorry, an error occurred while contacting the AI service.";
            }

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            // Protection from crashes
            if (!json.TryGetProperty("candidates", out var candidates) ||
                candidates.GetArrayLength() == 0)
            {
                return isArabic
                    ? "لم يتم الحصول على رد من الذكاء الاصطناعي."
                    : "No response from AI.";
            }

            var text = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ??
                   (isArabic
                       ? "عذرًا، لم أستطع إنشاء رد."
                       : "Sorry, I could not generate a response.");
        }

        private bool IsArabic(string text)
        {
            return text.Any(c => c >= 0x0600 && c <= 0x06FF);
        }
    }
}