using System.Net.Http.Json;
using APSPA_BakendAndFrontend.Server.DTOs.Prediction;

namespace APSPA_BakendAndFrontend.Server.Clients
{
    public class AiServiceClient : IAiServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AiServiceClient> _logger;

        public AiServiceClient(HttpClient httpClient, ILogger<AiServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AiPredictResponseDto> PredictAsync(AiPredictRequestDto request)
        {
            _logger.LogInformation("Sending prediction request to AI service at {BaseAddress}/predict",
                _httpClient.BaseAddress);

            var response = await _httpClient.PostAsJsonAsync("/predict", request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("AI service error: {StatusCode} - {Content}",
                    (int)response.StatusCode, responseContent);
                throw new HttpRequestException(
                    $"AI service returned {(int)response.StatusCode}: {responseContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<AiPredictResponseDto>();

            if (result == null)
                throw new InvalidOperationException("AI service returned an empty response.");

            _logger.LogInformation(
                "Prediction received: HR={HR}, EffortLevel={Effort}, FatigueRisk={Fatigue}",
                result.HeartRatePredicted, result.EffortLevel, result.FatigueRisk);

            return result;
        }
    }
}
