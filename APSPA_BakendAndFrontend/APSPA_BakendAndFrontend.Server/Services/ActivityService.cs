using APSPA_BakendAndFrontend.Server.DTOs.Activity;
using APSPA_BakendAndFrontend.Server.model;
using APSPA_BakendAndFrontend.Server.Repositories.Interfaces;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;

namespace APSPA_BakendAndFrontend.Server.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repo;

        public ActivityService(IActivityRepository repo)
        {
            _repo = repo;
        }

        public async Task<ActivityResponseDto> CreateAsync(int userId, CreateActivityDto dto)
        {
            var activity = new ActivityRecord
            {
                UserId              = userId,
                ActivityType        = dto.ActivityType,
                ActivityDurationMin = dto.ActivityDurationMin,
                ActivityDate        = dto.ActivityDate ?? DateTime.UtcNow,
                RestingHeartRate    = dto.RestingHeartRate,
                RecoveryScore       = dto.RecoveryScore,
                CaloriesBurned      = dto.CaloriesBurned,
                PerceivedIntensity  = dto.PerceivedIntensity,
                WeatherConditions   = dto.WeatherConditions,
                MeasuredHeartRate   = dto.MeasuredHeartRate,
                Notes               = dto.Notes,
                CreatedAt           = DateTime.UtcNow
            };

            await _repo.CreateAsync(activity);
            return ToDto(activity);
        }

        public async Task<List<ActivityResponseDto>> GetMyActivitiesAsync(int userId)
        {
            var list = await _repo.GetByUserIdAsync(userId);
            return list.Select(ToDto).ToList();
        }

        public async Task<ActivityResponseDto> GetByIdAsync(int userId, int activityId)
        {
            var activity = await _repo.GetByIdAsync(activityId)
                ?? throw new KeyNotFoundException($"Activity {activityId} not found.");

            if (activity.UserId != userId)
                throw new UnauthorizedAccessException("Access denied.");

            return ToDto(activity);
        }

        public async Task<ActivityResponseDto> UpdateAsync(int userId, int activityId, UpdateActivityDto dto)
        {
            var activity = await _repo.GetByIdAsync(activityId)
                ?? throw new KeyNotFoundException($"Activity {activityId} not found.");

            if (activity.UserId != userId)
                throw new UnauthorizedAccessException("Access denied.");

            if (dto.ActivityType != null) activity.ActivityType = dto.ActivityType;
            if (dto.ActivityDurationMin.HasValue) activity.ActivityDurationMin = dto.ActivityDurationMin.Value;
            if (dto.ActivityDate.HasValue) activity.ActivityDate = dto.ActivityDate.Value;
            if (dto.RestingHeartRate.HasValue) activity.RestingHeartRate = dto.RestingHeartRate;
            if (dto.RecoveryScore.HasValue) activity.RecoveryScore = dto.RecoveryScore;
            if (dto.CaloriesBurned.HasValue) activity.CaloriesBurned = dto.CaloriesBurned;
            if (dto.PerceivedIntensity != null) activity.PerceivedIntensity = dto.PerceivedIntensity;
            if (dto.WeatherConditions != null) activity.WeatherConditions = dto.WeatherConditions;
            if (dto.MeasuredHeartRate.HasValue) activity.MeasuredHeartRate = dto.MeasuredHeartRate;
            if (dto.Notes != null) activity.Notes = dto.Notes;
            activity.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(activity);
            return ToDto(activity);
        }

        public async Task DeleteAsync(int userId, int activityId)
        {
            var activity = await _repo.GetByIdAsync(activityId)
                ?? throw new KeyNotFoundException($"Activity {activityId} not found.");

            if (activity.UserId != userId)
                throw new UnauthorizedAccessException("Access denied.");

            await _repo.DeleteAsync(activity);
        }

        private static ActivityResponseDto ToDto(ActivityRecord a) => new()
        {
            Id                  = a.Id,
            UserId              = a.UserId,
            ActivityType        = a.ActivityType,
            ActivityDurationMin = a.ActivityDurationMin,
            ActivityDate        = a.ActivityDate,
            RestingHeartRate    = a.RestingHeartRate,
            RecoveryScore       = a.RecoveryScore,
            CaloriesBurned      = a.CaloriesBurned,
            PerceivedIntensity  = a.PerceivedIntensity,
            WeatherConditions   = a.WeatherConditions,
            MeasuredHeartRate   = a.MeasuredHeartRate,
            Notes               = a.Notes,
            CreatedAt           = a.CreatedAt
        };
    }
}
