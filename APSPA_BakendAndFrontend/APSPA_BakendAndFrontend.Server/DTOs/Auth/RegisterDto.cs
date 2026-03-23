using System.ComponentModel.DataAnnotations;

namespace APSPA_BakendAndFrontend.Server.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public int? Age { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        public decimal? WeightKg { get; set; }
        public decimal? HeightCm { get; set; }
    }
}
