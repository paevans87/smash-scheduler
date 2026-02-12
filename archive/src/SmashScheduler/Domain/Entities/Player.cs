using SmashScheduler.Domain.Enums;
using SQLite;

namespace SmashScheduler.Domain.Entities;

public class Player
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull, Indexed]
    public Guid ClubId { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    [NotNull]
    public int SkillLevel { get; set; }

    [NotNull]
    public Gender Gender { get; set; }

    [NotNull]
    public PlayStylePreference PlayStylePreference { get; set; }

    [NotNull]
    public DateTime CreatedAt { get; set; }

    [NotNull]
    public DateTime UpdatedAt { get; set; }

    [Ignore]
    public Club? Club { get; set; }

    [Ignore]
    public List<PlayerBlacklist> PartnerBlacklist { get; set; } = new();

    [Ignore]
    public List<PlayerBlacklist> OpponentBlacklist { get; set; } = new();

    [Ignore]
    public List<SessionPlayer> SessionPlayers { get; set; } = new();
}
