using SmashScheduler.Application.Services.Matchmaking.Models;

namespace SmashScheduler.Application.Services.Matchmaking;

public interface IMatchmakingService
{
    Task<List<MatchCandidate>> GenerateMatchesAsync(Guid sessionId, List<Guid>? excludePlayerIds = null, GenerationOptions? options = null);
    Task<MatchCandidate?> GenerateSingleMatchAsync(Guid sessionId, int courtNumber);
}
