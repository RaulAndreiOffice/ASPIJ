using APSPA_BakendAndFrontend.Server.DTOs.Profile;

namespace APSPA_BakendAndFrontend.Server.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileResponseDto> GetAsync(int userId);
        Task<ProfileResponseDto> UpdateAsync(int userId, UpdateProfileDto dto);
    }
}
