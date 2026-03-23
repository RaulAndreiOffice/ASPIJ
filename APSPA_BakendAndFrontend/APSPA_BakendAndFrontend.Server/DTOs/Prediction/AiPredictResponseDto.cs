using System.Text.Json.Serialization;

namespace APSPA_BakendAndFrontend.Server.DTOs.Prediction
{
    public class AiPredictResponseDto
    {
        [JsonPropertyName("heart_rate_predicted")]
        public float HeartRatePredicted { get; set; }

        [JsonPropertyName("difference")]
        public float? Difference { get; set; }

        [JsonPropertyName("is_anomaly")]
        public bool? IsAnomaly { get; set; }

        [JsonPropertyName("effort_level")]
        public string EffortLevel { get; set; } = string.Empty;

        [JsonPropertyName("fatigue_risk")]
        public string FatigueRisk { get; set; } = string.Empty;

        [JsonPropertyName("recommendation")]
        public string Recommendation { get; set; } = string.Empty;
    }
}
