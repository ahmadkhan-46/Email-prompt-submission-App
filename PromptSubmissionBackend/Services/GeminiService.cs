using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PromptSubmissionBackend.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeminiService> _logger;
        private readonly string _apiKey;

        public GeminiService(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<GeminiService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;

            _apiKey = config["Gemini:ApiKey"] ?? throw new ArgumentException("Gemini API key is not configured.");
        }

        public async Task<string> GenerateEmailFromPromptAsync(string promptText)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = $"Convert this prompt into a professional, formal email:\n\n{promptText}" }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

            try
            {
                _logger.LogInformation("Sending request to Gemini API...");
                var response = await _httpClient.PostAsync(url, content);

                _logger.LogInformation("Gemini API Response Status: {StatusCode}", response.StatusCode);

                var resultJson = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Gemini API Raw Response: {Response}", resultJson);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Gemini API call failed. Status: {StatusCode}, Response: {Response}", response.StatusCode, resultJson);
                    return $"Error from Gemini API: {response.StatusCode}";
                }

                var result = JsonConvert.DeserializeObject<JObject>(resultJson);
                var generatedText = result?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                if (string.IsNullOrWhiteSpace(generatedText))
                {
                    _logger.LogWarning("Gemini API returned empty text for the prompt.");
                    return "Could not generate email text.";
                }

                return generatedText;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling Gemini API.");
                return "An error occurred while generating email content.";
            }
        }
    }
}
