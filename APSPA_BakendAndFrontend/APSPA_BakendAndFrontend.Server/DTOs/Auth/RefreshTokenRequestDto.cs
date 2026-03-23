using System.ComponentModel.DataAnnotations;

namespace APSPA_BakendAndFrontend.Server.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
