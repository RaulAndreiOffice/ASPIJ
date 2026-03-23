using System.Text.Json.Serialization;

namespace APSPA_BakendAndFrontend.Server.DTOs.Prediction
{
    public class AiPredictRequestDto
    {
        [JsonPropertyName("Actual_Weight")]
        public float ActualWeight { get; set; }

        [JsonPropertyName("Age")]
        public int Age { get; set; }

        [JsonPropertyName("Duration")]
        public float Duration { get; set; }

        [JsonPropertyName("BMI")]
        public float BMI { get; set; }

        [JsonPropertyName("Exercise_Intensity")]
        public float ExerciseIntensity { get; set; }

        [JsonPropertyName("Gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonPropertyName("Exercise")]
        public string Exercise { get; set; } = string.Empty;

        [JsonPropertyName("Weather_Conditions")]
        public string WeatherConditions { get; set; } = string.Empty;

        [JsonPropertyName("HeartRate_Measured")]
        public float? HeartRateMeasured { get; set; }
    }
}
