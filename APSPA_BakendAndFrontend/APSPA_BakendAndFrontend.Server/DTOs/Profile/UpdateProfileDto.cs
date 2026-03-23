using System.ComponentModel.DataAnnotations;

namespace APSPA_BakendAndFrontend.Server.DTOs.Profile
{
    public class UpdateProfileDto
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [Range(1, 120)]
        public int? Age { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
    }
}
