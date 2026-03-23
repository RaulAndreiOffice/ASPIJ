using System.Diagnostics;
using System.Text.Json;
using APSPA_BakendAndFrontend.Server.Clients;
using APSPA_BakendAndFrontend.Server.Data;
using APSPA_BakendAndFrontend.Server.DTOs.Prediction;
using APSPA_BakendAndFrontend.Server.model;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APSPA_BakendAndFrontend.Server.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly AppDbContext _context;
        private readonly IAiServiceClient _aiServiceClient;
        private readonly ILogger<PredictionService> _logger;

        public PredictionService(
            AppDbContext context,
            IAiServiceClient aiServiceClient,
            ILogger<PredictionService> logger)
        {
            _context = context;
            _aiServiceClient = aiServiceClient;
            _logger = logger;
        }

        public async Task<Prediction> RunPredictionAsync(int activityId)
        {
            var activity = await _context.ActivityRecords
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == activityId)
                ?? throw new KeyNotFoundException($"Activity {activityId} not found.");

            var user = activity.User;

            var request = new AiPredictRequestDto
            {
                ActualWeight = user.WeightKg.HasValue ? (float)user.WeightKg.Value : 0f,
                Age          = user.Age ?? 0,
                Duration     = activity.ActivityDurationMin,
                BMI          = CalculateBmi(user.WeightKg, user.HeightCm),
                ExerciseIntensity = MapIntensity(activity.PerceivedIntensity),
                Gender           = user.Gender ?? "Unknown",
                Exercise         = activity.ActivityType,
                WeatherConditions = activity.WeatherConditions ?? "Sunny",
                HeartRateMeasured = activity.MeasuredHeartRate.HasValue
                    ? activity.MeasuredHeartRate
                    : activity.RestingHeartRate.HasValue
                        ? (float?)activity.RestingHeartRate.Value
                        : null
            };

            // Persist prediction record immediately (status = Pending)
            var prediction = new Prediction
            {
                UserId           = user.Id,
                ActivityRecordId = activity.Id,
                PredictionStatus = "Pending",
                RawInputPayload  = JsonSerializer.Serialize(request),
                CreatedAt        = DateTime.UtcNow
            };

            _context.Predictions.Add(prediction);
            await _context.SaveChangesAsync();

            var sw = Stopwatch.StartNew();
            try
            {
                var aiResponse = await _aiServiceClient.PredictAsync(request);
                sw.Stop();

                prediction.PredictedAvgHeartRate = (decimal)aiResponse.HeartRatePredicted;
                prediction.EffortLevel           = aiResponse.EffortLevel;
                prediction.FatigueRisk           = aiResponse.FatigueRisk;
                prediction.Recommendation        = aiResponse.Recommendation;
                prediction.Difference            = aiResponse.Difference.HasValue ? (decimal)aiResponse.Difference.Value : null;
                prediction.IsAnomaly             = aiResponse.IsAnomaly;
                prediction.PredictionStatus      = "Completed";
                prediction.RawOutputPayload      = JsonSerializer.Serialize(aiResponse);
                prediction.CompletedAt           = DateTime.UtcNow;

                // Audit log for the AI call
                _context.AiRequestLogs.Add(new AiRequestLog
                {
                    PredictionId    = prediction.Id,
                    Endpoint        = "/predict",
                    HttpMethod      = "POST",
                    RequestPayload  = JsonSerializer.Serialize(request),
                    ResponsePayload = JsonSerializer.Serialize(aiResponse),
                    ResponseStatus  = 200,
                    DurationMs      = (int)sw.ElapsedMilliseconds,
                    IsSuccess       = true,
                    CreatedAt       = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                return prediction;
            }
            catch (Exception ex)
            {
                sw.Stop();

                prediction.PredictionStatus = "Failed";
                prediction.ErrorMessage     = ex.Message;
                prediction.CompletedAt      = DateTime.UtcNow;

                _context.AiRequestLogs.Add(new AiRequestLog
                {
                    PredictionId    = prediction.Id,
                    Endpoint        = "/predict",
                    HttpMethod      = "POST",
                    RequestPayload  = JsonSerializer.Serialize(request),
                    ResponsePayload = null,
                    ResponseStatus  = 500,
                    DurationMs      = (int)sw.ElapsedMilliseconds,
                    IsSuccess       = false,
                    CreatedAt       = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                _logger.LogError(ex, "Prediction failed for activity {ActivityId}", activityId);
                throw;
            }
        }

        // ------------------------------------------------------------------ helpers

        private static float CalculateBmi(decimal? weightKg, decimal? heightCm)
        {
            if (weightKg == null || heightCm == null || heightCm <= 0)
                return 0f;

            var heightM = (float)heightCm.Value / 100f;
            return (float)weightKg.Value / (heightM * heightM);
        }

        private static float MapIntensity(string? perceived) =>
            perceived?.ToLower() switch
            {
                "low"       => 3f,
                "medium"    => 6f,
                "high"      => 8f,
                "very high" => 10f,
                _           => 5f
            };
    }
}
