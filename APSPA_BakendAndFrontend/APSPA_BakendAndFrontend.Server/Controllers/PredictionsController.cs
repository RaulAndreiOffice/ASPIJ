using System.Security.Claims;
using APSPA_BakendAndFrontend.Server.DTOs.Prediction;
using APSPA_BakendAndFrontend.Server.Repositories.Interfaces;
using APSPA_BakendAndFrontend.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APSPA_BakendAndFrontend.Server.Controllers
{
    [ApiController]
    [Route("api/predictions")]
    [Authorize]
    public class PredictionsController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        private readonly IPredictionRepository _predictionRepository;
        private readonly ILogger<PredictionsController> _logger;

        public PredictionsController(
            IPredictionService predictionService,
            IPredictionRepository predictionRepository,
            ILogger<PredictionsController> logger)
        {
            _predictionService = predictionService;
            _predictionRepository = predictionRepository;
            _logger = logger;
        }

        /// <summary>
        /// Runs an AI prediction for the given activity.
        /// Calls the FastAPI AI service and persists the result in the DB.
        /// </summary>
        [HttpPost("run/{activityId:int}")]
        public async Task<IActionResult> RunPrediction(int activityId)
        {
            try
            {
                var prediction = await _predictionService.RunPredictionAsync(activityId);

                return Ok(new PredictionResponseDto
                {
                    Id                    = prediction.Id,
                    ActivityRecordId      = prediction.ActivityRecordId,
                    UserId                = prediction.UserId,
                    PredictedAvgHeartRate = prediction.PredictedAvgHeartRate,
                    EffortLevel           = prediction.EffortLevel,
                    FatigueRisk           = prediction.FatigueRisk,
                    Recommendation        = prediction.Recommendation,
                    Difference            = prediction.Difference,
                    IsAnomaly             = prediction.IsAnomaly,
                    PredictionStatus      = prediction.PredictionStatus,
                    ErrorMessage          = prediction.ErrorMessage,
                    CreatedAt             = prediction.CreatedAt,
                    CompletedAt           = prediction.CompletedAt
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Prediction failed for activity {ActivityId}", activityId);
                return StatusCode(500, new { error = "Prediction failed.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Returns all predictions for the current authenticated user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            var userId = GetUserId();
            var list = await _predictionRepository.GetByUserIdAsync(userId);
            var result = list.Select(p => new PredictionResponseDto
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
            return Ok(result);
        }

        /// <summary>
        /// Returns a single prediction by ID.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prediction = await _predictionRepository.GetByIdAsync(id);
            if (prediction == null)
                return NotFound(new { error = $"Prediction {id} not found." });

            return Ok(new PredictionResponseDto
            {
                Id                    = prediction.Id,
                ActivityRecordId      = prediction.ActivityRecordId,
                UserId                = prediction.UserId,
                PredictedAvgHeartRate = prediction.PredictedAvgHeartRate,
                EffortLevel           = prediction.EffortLevel,
                FatigueRisk           = prediction.FatigueRisk,
                Recommendation        = prediction.Recommendation,
                Difference            = prediction.Difference,
                IsAnomaly             = prediction.IsAnomaly,
                PredictionStatus      = prediction.PredictionStatus,
                ErrorMessage          = prediction.ErrorMessage,
                CreatedAt             = prediction.CreatedAt,
                CompletedAt           = prediction.CompletedAt
            });
        }

        /// <summary>
        /// Returns all predictions for a specific activity.
        /// </summary>
        [HttpGet("activity/{activityId:int}")]
        public async Task<IActionResult> GetByActivity(int activityId)
        {
            var list = await _predictionRepository.GetByActivityIdAsync(activityId);
            var result = list.Select(p => new PredictionResponseDto
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
            return Ok(result);
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}
