using APSPA_BakendAndFrontend.Server.DTOs.Activity;
using APSPA_BakendAndFrontend.Server.DTOs.Prediction;
using APSPA_BakendAndFrontend.Server.DTOs.Profile;

namespace APSPA_BakendAndFrontend.Server.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<ProfileResponseDto>> GetAllUsersAsync();
        Task<List<ActivityResponseDto>> GetAllActivitiesAsync();
        Task<List<PredictionResponseDto>> GetAllPredictionsAsync();
    }
}
