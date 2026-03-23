using APSPA_BakendAndFrontend.Server.model;

namespace APSPA_BakendAndFrontend.Server.Repositories.Interfaces
{
    public interface IPredictionRepository
    {
        Task<Prediction?> GetByIdAsync(int id);
        Task<List<Prediction>> GetByUserIdAsync(int userId);
        Task<List<Prediction>> GetByActivityIdAsync(int activityId);
        Task<List<Prediction>> GetAllAsync();
    }
}
