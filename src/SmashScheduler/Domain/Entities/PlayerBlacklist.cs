using SmashScheduler.Domain.Enums;
using SQLite;

namespace SmashScheduler.Domain.Entities;

public class PlayerBlacklist
{
    [Indexed]
    public Guid PlayerId { get; set; }

    [Indexed]
    public Guid BlacklistedPlayerId { get; set; }

    [NotNull]
    public BlacklistType BlacklistType { get; set; }

    [NotNull]
    public DateTime CreatedAt { get; set; }

    [Ignore]
    public Player? Player { get; set; }

    [Ignore]
    public Player? BlacklistedPlayer { get; set; }
}
