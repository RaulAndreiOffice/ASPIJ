namespace APSPA_BakendAndFrontend.Server.DTOs.Activity
{
    public class ActivityResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public int ActivityDurationMin { get; set; }
        public DateTime ActivityDate { get; set; }
        public int? RestingHeartRate { get; set; }
        public int? RecoveryScore { get; set; }
        public int? CaloriesBurned { get; set; }
        public string? PerceivedIntensity { get; set; }
        public string? WeatherConditions { get; set; }
        public float? MeasuredHeartRate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
