using System.ComponentModel.DataAnnotations;

namespace APSPA_BakendAndFrontend.Server.DTOs.Activity
{
    public class UpdateActivityDto
    {
        [MaxLength(100)]
        public string? ActivityType { get; set; }

        [Range(1, 1440)]
        public int? ActivityDurationMin { get; set; }

        public DateTime? ActivityDate { get; set; }

        [Range(30, 250)]
        public int? RestingHeartRate { get; set; }

        [Range(0, 100)]
        public int? RecoveryScore { get; set; }

        public int? CaloriesBurned { get; set; }

        [MaxLength(50)]
        public string? PerceivedIntensity { get; set; }

        [MaxLength(100)]
        public string? WeatherConditions { get; set; }

        [Range(30, 250)]
        public float? MeasuredHeartRate { get; set; }

        public string? Notes { get; set; }
    }
}
