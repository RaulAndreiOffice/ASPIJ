using APSPA_BakendAndFrontend.Server.DTOs.Activity;
using APSPA_BakendAndFrontend.Server.DTOs.Prediction;
using APSPA_BakendAndFrontend.Server.DTOs.Profile;
using APSPA_BakendAndFrontend.Server.Repositories.Interfaces;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;

namespace APSPA_BakendAndFrontend.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _users;
        private readonly IActivityRepository _activities;
        private readonly IPredictionRepository _predictions;

        public AdminService(IUserRepository users, IActivityRepository activities, IPredictionRepository predictions)
        {
            _users = users;
            _activities = activities;
            _predictions = predictions;
        }

        public async Task<List<ProfileResponseDto>> GetAllUsersAsync()
        {
            var list = await _users.GetAllAsync();
            return list.Select(u => new ProfileResponseDto
            {
                Id        = u.Id,
                FirstName = u.FirstName,
                LastName  = u.LastName,
                Email     = u.Email,
                Age       = u.Age,
                Gender    = u.Gender,
                WeightKg  = u.WeightKg,
                HeightCm  = u.HeightCm,
                Role      = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task<List<ActivityResponseDto>> GetAllActivitiesAsync()
        {
            var list = await _activities.GetAllAsync();
            return list.Select(a => new ActivityResponseDto
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
            }).ToList();
        }

        public async Task<List<PredictionResponseDto>> GetAllPredictionsAsync()
        {
            var list = await _predictions.GetAllAsync();
            return list.Select(p => new PredictionResponseDto
            {
                Id                    = p.Id,
                ActivityRecordId      = p.ActivityRecordId,
                UserId                = p.UserId,
                PredictedAvgHeartRate = p.PredictedAvgHeartRate,
                EffortLevel           = p.EffortLevel,
                FatigueRisk           = p.FatigueRisk,
                Recommendation        = p.Recommendation,
                Difference            = p.Difference,
                IsAnomaly             = p.IsAnomaly,
                PredictionStatus      = p.PredictionStatus,
                ErrorMessage          = p.ErrorMessage,
                CreatedAt             = p.CreatedAt,
                CompletedAt           = p.CompletedAt
            }).ToList();
        }
    }
}
