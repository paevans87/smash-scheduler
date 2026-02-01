using SmashScheduler.Domain.Enums;
using SQLite;

namespace SmashScheduler.Domain.Entities;

public class Club
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    [NotNull]
    public int DefaultCourtCount { get; set; }

    [NotNull]
    public GameType GameType { get; set; }

    [NotNull]
    public DateTime CreatedAt { get; set; }

    [NotNull]
    public DateTime UpdatedAt { get; set; }

    [Ignore]
    public List<Player> Players { get; set; } = new();

    [Ignore]
    public List<Session> Sessions { get; set; } = new();
}
