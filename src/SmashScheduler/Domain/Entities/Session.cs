using SmashScheduler.Domain.Enums;
using SQLite;

namespace SmashScheduler.Domain.Entities;

public class Session
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull, Indexed]
    public Guid ClubId { get; set; }

    [NotNull]
    public DateTime ScheduledDateTime { get; set; }

    [NotNull]
    public int CourtCount { get; set; }

    [NotNull, Indexed]
    public SessionState State { get; set; }

    [NotNull]
    public DateTime CreatedAt { get; set; }

    [NotNull]
    public DateTime UpdatedAt { get; set; }

    [Ignore]
    public Club? Club { get; set; }

    [Ignore]
    public List<SessionPlayer> SessionPlayers { get; set; } = new();

    [Ignore]
    public List<Match> Matches { get; set; } = new();

    [Ignore]
    public IEnumerable<Player> BenchedPlayers
    {
        get
        {
            var activePlayers = SessionPlayers
                .Where(sp => sp.IsActive)
                .Select(sp => sp.PlayerId)
                .ToHashSet();

            var playersInMatches = Matches
                .Where(m => m.State == MatchState.InProgress)
                .SelectMany(m => m.PlayerIds)
                .ToHashSet();

            return SessionPlayers
                .Where(sp => sp.IsActive && !playersInMatches.Contains(sp.PlayerId))
                .Select(sp => sp.Player!)
                .Where(p => p != null);
        }
    }

    [Ignore]
    public IEnumerable<Player> ActivePlayers => SessionPlayers
        .Where(sp => sp.IsActive)
        .Select(sp => sp.Player!)
        .Where(p => p != null);
}
