//using eLab.DAL.Models;
//using Stripe;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Json;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace eLab.BLL.Services.Classes
//{
//    public class GeminiAIService : IAIService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string _apiKey;

//        public GeminiAIService(HttpClient httpClient, IConfiguration config)
//        {
//            _httpClient = httpClient;
//            _apiKey = config["Gemini:ApiKey"]!;
//        }

//        public async Task<string> GetConversationResponseAsync(
//            string contextSummary,
//            IEnumerable<AIChatMessage> history,
//            string newMessage)
//        {
//            // build message list: system context + conversation history
//            var messages = new List<object>
//        {
//            new { role = "user", parts = new[] { new { text = contextSummary } } },
//            new { role = "model", parts = new[] { new { text = "Understood. I will help explain the lab results." } } }
//        };

//            foreach (var msg in history)
//            {
//                messages.Add(new
//                {
//                    role = msg.Role == "user" ? "user" : "model",
//                    parts = new[] { new { text = msg.Message } }
//                });
//            }

//            var body = new { contents = messages };
//            var url = $"v1beta/models/gemini-pro:generateContent?key={_apiKey}";

//            var response = await _httpClient.PostAsJsonAsync(url, body);
//            response.EnsureSuccessStatusCode();

//            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
//            return json
//                .GetProperty("candidates")[0]
//                .GetProperty("content")
//                .GetProperty("parts")[0]
//                .GetProperty("text")
//                .GetString() ?? "Sorry, I could not generate a response.";
//        }
//    }
//}
