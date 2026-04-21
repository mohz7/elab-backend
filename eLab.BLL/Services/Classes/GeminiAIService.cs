using eLab.BLL.Services.Interface;
using eLab.DAL.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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

            _model = config["Gemini:Model"] ?? "gemini-1.5-flash";
        }

        public async Task<string> GetConversationResponseAsync(
            string contextSummary,
            IEnumerable<AIChatMessage> history,
            string newMessage)
        {
            var contents = new List<object>();

            // context
            contents.Add(new
            {
                role = "user",
                parts = new[] { new { text = contextSummary } }
            });

            contents.Add(new
            {
                role = "model",
                parts = new[]
                {
                    new
                    {
                        text = "Understood. I will help explain these lab results in simple language. Please ask your question."
                    }
                }
            });

            // history
            foreach (var msg in history)
            {
                contents.Add(new
                {
                    role = msg.Role == ChatMessageRole.User ? "user" : "model",
                    parts = new[] { new { text = msg.Message } }
                });
            }

            // new message
            contents.Add(new
            {
                role = "user",
                parts = new[] { new { text = newMessage } }
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

            var url = $"v1beta/models/{_model}:generateContent?key={_apiKey}";

            var response = await _httpClient.PostAsJsonAsync(url, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error ({response.StatusCode}): {error}");
            }

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            // حماية من الكراش
            if (!json.TryGetProperty("candidates", out var candidates) ||
                candidates.GetArrayLength() == 0)
            {
                return "No response from AI.";
            }

            var text = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "Sorry, I could not generate a response. Please try again.";
        }
    }
}