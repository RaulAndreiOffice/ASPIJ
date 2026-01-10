namespace APSPA_BakendAndFrontend.Server.model
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        public string Role { get; set; } = "athlete";  // athlete / coach / admin

        public int? Age { get; set; }
        public string? Gender { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? Bmi { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relații
        public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}

