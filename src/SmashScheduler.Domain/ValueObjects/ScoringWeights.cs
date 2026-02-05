namespace SmashScheduler.Domain.ValueObjects;

public class ScoringWeights
{
    public int SkillBalance { get; set; } = 40;
    public int MatchHistory { get; set; } = 35;
    public int TimeOffCourt { get; set; } = 25;

    public bool IsValid => SkillBalance + MatchHistory + TimeOffCourt == 100
                           && SkillBalance >= 0
                           && MatchHistory >= 0
                           && TimeOffCourt >= 0;
}
