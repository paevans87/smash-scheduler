using SQLite;

namespace SmashScheduler.Domain.Entities;

public class SessionPlayer
{
    [Indexed]
    public Guid SessionId { get; set; }

    [Indexed]
    public Guid PlayerId { get; set; }

    [NotNull]
    public bool IsActive { get; set; }

    [NotNull]
    public DateTime JoinedAt { get; set; }

    [Ignore]
    public Session? Session { get; set; }

    [Ignore]
    public Player? Player { get; set; }
}
