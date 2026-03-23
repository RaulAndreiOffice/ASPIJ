namespace APSPA_BakendAndFrontend.Server.DTOs.Prediction
{
    public class PredictionResponseDto
    {
        public int Id { get; set; }
        public int ActivityRecordId { get; set; }
        public int UserId { get; set; }
        public decimal? PredictedAvgHeartRate { get; set; }
        public string? EffortLevel { get; set; }
        public string? FatigueRisk { get; set; }
        public string? Recommendation { get; set; }
        public decimal? Difference { get; set; }
        public bool? IsAnomaly { get; set; }
        public string PredictionStatus { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
