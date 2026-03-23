using APSPA_BakendAndFrontend.Server.DTOs.Activity;

namespace APSPA_BakendAndFrontend.Server.Services.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityResponseDto> CreateAsync(int userId, CreateActivityDto dto);
        Task<List<ActivityResponseDto>> GetMyActivitiesAsync(int userId);
        Task<ActivityResponseDto> GetByIdAsync(int userId, int activityId);
        Task<ActivityResponseDto> UpdateAsync(int userId, int activityId, UpdateActivityDto dto);
        Task DeleteAsync(int userId, int activityId);
    }
}
