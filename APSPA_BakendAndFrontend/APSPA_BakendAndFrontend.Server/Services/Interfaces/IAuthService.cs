using APSPA_BakendAndFrontend.Server.DTOs.Auth;

namespace APSPA_BakendAndFrontend.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> RegisterAsync(RegisterDto dto);
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<LoginResponseDto> RefreshAsync(string refreshToken);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}
