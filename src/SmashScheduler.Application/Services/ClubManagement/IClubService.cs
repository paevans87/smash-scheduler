using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
using SmashScheduler.Domain.ValueObjects;

namespace SmashScheduler.Application.Services.ClubManagement;

public interface IClubService
{
    Task<Club?> GetByIdAsync(Guid id);
    Task<List<Club>> GetAllClubsAsync();
    Task<Club> CreateClubAsync(string name, int defaultCourtCount, GameType gameType, ScoringWeights scoringWeights, BlacklistMode blacklistMode);
    Task UpdateClubAsync(Club club);
    Task DeleteClubAsync(Guid id);
}
