using APSPA_BakendAndFrontend.Server.model;

namespace APSPA_BakendAndFrontend.Server.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<Prediction> RunPredictionAsync(int activityId);
    }
}
