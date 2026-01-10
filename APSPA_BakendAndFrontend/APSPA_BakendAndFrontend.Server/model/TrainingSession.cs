using System.Diagnostics.Metrics;

namespace APSPA_BakendAndFrontend.Server.model
{
    public class TrainingSession
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? DeviceId { get; set; }
        public Device? Device { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? DurationMin { get; set; }

        public string? ExerciseType { get; set; }          // "Exercise 1" etc.
        public int? ExerciseIntensity { get; set; }        // 1-10
        public string? Weather { get; set; }               // "Sunny", etc.

        public decimal? CaloriesBurned { get; set; }
        public decimal? HeartRateAvg { get; set; }
        public decimal? HeartRateMax { get; set; }

        public decimal? BmiSnapshot { get; set; }
        public decimal? WeightSnapshot { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
        public ICollection<Anomaly> Anomalies { get; set; } = new List<Anomaly>();
    }
}

