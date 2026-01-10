namespace APSPA_BakendAndFrontend.Server.model
{
    public class Measurement
    {
        public int Id { get; set; }

        public int SessionId { get; set; }
        public TrainingSession Session { get; set; } = null!;

        public DateTime MeasuredAt { get; set; }

        public decimal? HeartRate { get; set; }
        public decimal? Spo2 { get; set; }
        public decimal? RawValue { get; set; }

        public ICollection<Anomaly> Anomalies { get; set; } = new List<Anomaly>();
    }
}

