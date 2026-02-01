using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.Matchmaking.Models;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Application.Services.Matchmaking;

public class MatchmakingService(
    ISessionRepository sessionRepository,
    IPlayerRepository playerRepository,
    IMatchRepository matchRepository,
    IClubRepository clubRepository) : IMatchmakingService
{
    public async Task<List<MatchCandidate>> GenerateMatchesAsync(Guid sessionId)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId);
        if (session == null) throw new InvalidOperationException("Session not found");

        var club = await clubRepository.GetByIdAsync(session.ClubId);
        if (club == null) throw new InvalidOperationException("Club not found");

        var candidates = new List<MatchCandidate>();
        
        return candidates;
    }
}
