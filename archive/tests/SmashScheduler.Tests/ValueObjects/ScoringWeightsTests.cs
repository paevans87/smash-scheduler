using FluentAssertions;
using SmashScheduler.Domain.ValueObjects;
using Xunit;

namespace SmashScheduler.Tests.ValueObjects;

public class ScoringWeightsTests
{
    [Fact]
    public void DefaultWeights_AreValid()
    {
        var weights = new ScoringWeights();

        weights.IsValid.Should().BeTrue();
        weights.SkillBalance.Should().Be(40);
        weights.MatchHistory.Should().Be(35);
        weights.TimeOffCourt.Should().Be(25);
    }

    [Fact]
    public void Weights_ThatAddToHundred_AreValid()
    {
        var weights = new ScoringWeights
        {
            SkillBalance = 60,
            MatchHistory = 20,
            TimeOffCourt = 20
        };

        weights.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Weights_ThatDoNotAddToHundred_AreInvalid()
    {
        var weights = new ScoringWeights
        {
            SkillBalance = 50,
            MatchHistory = 30,
            TimeOffCourt = 30
        };

        weights.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Weights_WithNegativeValues_AreInvalid()
    {
        var weights = new ScoringWeights
        {
            SkillBalance = -10,
            MatchHistory = 60,
            TimeOffCourt = 50
        };

        weights.IsValid.Should().BeFalse();
    }
}
