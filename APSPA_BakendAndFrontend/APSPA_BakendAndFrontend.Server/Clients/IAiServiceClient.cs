using APSPA_BakendAndFrontend.Server.DTOs.Prediction;

namespace APSPA_BakendAndFrontend.Server.Clients
{
    public interface IAiServiceClient
    {
        Task<AiPredictResponseDto> PredictAsync(AiPredictRequestDto request);
    }
}
