using SmashScheduler.Domain.Enums;
using SmashScheduler.Domain.ValueObjects;
using SQLite;

namespace SmashScheduler.Domain.Entities;

public class Match
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull, Indexed]
    public Guid SessionId { get; set; }

    [NotNull]
    public int CourtNumber { get; set; }

    [NotNull, Indexed]
    public MatchState State { get; set; }

    [NotNull]
    public bool WasAutomated { get; set; }

    [NotNull]
    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? ScoreJson { get; set; }

    public string? PlayerIdsJson { get; set; }

    public string? WinningPlayerIdsJson { get; set; }

    [Ignore]
    public Session? Session { get; set; }

    [Ignore]
    public List<Player> Players { get; set; } = new();

    [Ignore]
    public List<Player> WinningPlayers { get; set; } = new();

    [Ignore]
    public List<Guid> PlayerIds
    {
        get
        {
            if (string.IsNullOrEmpty(PlayerIdsJson))
            {
                return new List<Guid>();
            }

            return System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(PlayerIdsJson) ?? new List<Guid>();
        }
        set
        {
            PlayerIdsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }

    [Ignore]
    public List<Guid> WinningPlayerIds
    {
        get
        {
            if (string.IsNullOrEmpty(WinningPlayerIdsJson))
            {
                return new List<Guid>();
            }

            return System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(WinningPlayerIdsJson) ?? new List<Guid>();
        }
        set
        {
            WinningPlayerIdsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }

    [Ignore]
    public MatchScore? Score
    {
        get
        {
            if (string.IsNullOrEmpty(ScoreJson))
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<MatchScore>(ScoreJson);
        }
        set
        {
            ScoreJson = value == null ? null : System.Text.Json.JsonSerializer.Serialize(value);
        }
    }
}
