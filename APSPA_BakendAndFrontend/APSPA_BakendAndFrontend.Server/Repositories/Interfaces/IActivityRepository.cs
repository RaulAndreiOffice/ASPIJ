using APSPA_BakendAndFrontend.Server.model;

namespace APSPA_BakendAndFrontend.Server.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task<ActivityRecord?> GetByIdAsync(int id);
        Task<List<ActivityRecord>> GetByUserIdAsync(int userId);
        Task<List<ActivityRecord>> GetAllAsync();
        Task<ActivityRecord> CreateAsync(ActivityRecord activity);
        Task UpdateAsync(ActivityRecord activity);
        Task DeleteAsync(ActivityRecord activity);
    }
}
