using SmashScheduler.Domain.Enums;
using SmashScheduler.Domain.ValueObjects;

namespace SmashScheduler.Domain.Entities;

public class Club
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DefaultCourtCount { get; set; }
    public GameType GameType { get; set; }
    public BlacklistMode BlacklistMode { get; set; } = BlacklistMode.Preferred;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? ScoringWeightsJson { get; set; }

    public ScoringWeights ScoringWeights
    {
        get
        {
            if (string.IsNullOrEmpty(ScoringWeightsJson))
            {
                return new ScoringWeights();
            }

            return System.Text.Json.JsonSerializer.Deserialize<ScoringWeights>(ScoringWeightsJson) ?? new ScoringWeights();
        }
        set
        {
            ScoringWeightsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }

    public List<Player> Players { get; set; } = new();
    public List<Session> Sessions { get; set; } = new();
}
