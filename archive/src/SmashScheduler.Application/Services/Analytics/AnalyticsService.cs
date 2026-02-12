using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.Analytics.Models;
using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Application.Services.Analytics;

public class AnalyticsService(
    ISessionRepository sessionRepository,
    IMatchRepository matchRepository) : IAnalyticsService
{
    public async Task<SessionStatistics> GetSessionStatisticsAsync(Guid sessionId)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId);
        if (session == null) throw new InvalidOperationException("Session not found");

        var matches = await matchRepository.GetBySessionIdAsync(sessionId);

        var completedMatches = matches.Where(m => m.State == MatchState.Completed).ToList();
        var automatedMatches = matches.Count(m => m.WasAutomated);
        var manualMatches = matches.Count(m => !m.WasAutomated);

        var gamesPlayedPerPlayer = new Dictionary<Guid, int>();
        var playTimePerPlayer = new Dictionary<Guid, TimeSpan>();

        foreach (var match in matches)
        {
            var matchDuration = match.CompletedAt.HasValue
                ? match.CompletedAt.Value - match.StartedAt
                : DateTime.UtcNow - match.StartedAt;

            foreach (var playerId in match.PlayerIds)
            {
                gamesPlayedPerPlayer.TryGetValue(playerId, out var currentGames);
                gamesPlayedPerPlayer[playerId] = currentGames + 1;

                playTimePerPlayer.TryGetValue(playerId, out var currentTime);
                playTimePerPlayer[playerId] = currentTime + matchDuration;
            }
        }

        var totalGameTime = completedMatches
            .Where(m => m.CompletedAt.HasValue)
            .Aggregate(TimeSpan.Zero, (total, m) => total + (m.CompletedAt!.Value - m.StartedAt));

        var overrideRate = matches.Count > 0 ? (double)manualMatches / matches.Count : 0;

        return new SessionStatistics
        {
            SessionId = sessionId,
            TotalMatches = matches.Count,
            CompletedMatches = completedMatches.Count,
            AutomatedMatches = automatedMatches,
            ManualMatches = manualMatches,
            OverrideRate = overrideRate,
            TotalGameTime = totalGameTime,
            GamesPlayedPerPlayer = gamesPlayedPerPlayer,
            PlayTimePerPlayer = playTimePerPlayer
        };
    }

    public Task<PlayerStatistics> GetPlayerStatisticsAsync(Guid playerId)
    {
        return Task.FromResult(new PlayerStatistics
        {
            PlayerId = playerId,
            TotalGamesPlayed = 0
        });
    }
}
