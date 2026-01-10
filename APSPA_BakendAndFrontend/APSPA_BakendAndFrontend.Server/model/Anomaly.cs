namespace APSPA_BakendAndFrontend.Server.model
{
    public class Anomaly
    {
        public int Id { get; set; }

        public int? SessionId { get; set; }
        public TrainingSession? Session { get; set; }

        public int? MeasurementId { get; set; }
        public Measurement? Measurement { get; set; }

        public decimal? PredictedHr { get; set; }
        public decimal? MeasuredHr { get; set; }
        public decimal? DiffHr { get; set; }

        public string? RiskLevel { get; set; }      // "low", "medium", "high"
        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
