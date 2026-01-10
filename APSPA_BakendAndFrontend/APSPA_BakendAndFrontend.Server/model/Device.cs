namespace APSPA_BakendAndFrontend.Server.model
{
    public class Device
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public string? Name { get; set; }
        public string? SerialNumber { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastSeen { get; set; }

        public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
    }
}

