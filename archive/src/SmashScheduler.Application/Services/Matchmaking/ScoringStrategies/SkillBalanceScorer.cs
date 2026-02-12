using SmashScheduler.Application.Services.Matchmaking.Models;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Application.Services.Matchmaking.ScoringStrategies;

public class SkillBalanceScorer : IMatchScorer
{
    public double CalculateScore(MatchCandidate candidate, List<Player> allPlayers, MatchScoringContext context)
    {
        var players = allPlayers.Where(p => candidate.PlayerIds.Contains(p.Id)).ToList();

        if (players.Count != 4)
        {
            return 0;
        }

        var (bestDifference, bestOrder) = CalculateBestTeamBalance(players);

        candidate.PlayerIds = bestOrder.Select(p => p.Id).ToList();

        var maxPossibleDifference = 18.0;
        var normalisedDifference = bestDifference / maxPossibleDifference;

        return (1.0 - normalisedDifference) * 100.0;
    }

    private (int difference, List<Player> orderedPlayers) CalculateBestTeamBalance(List<Player> players)
    {
        var teamSplits = new[]
        {
            new[] { 0, 1, 2, 3 },
            new[] { 0, 2, 1, 3 },
            new[] { 0, 3, 1, 2 }
        };

        var minDifference = int.MaxValue;
        var bestOrder = players;

        foreach (var indices in teamSplits)
        {
            var team1Total = players[indices[0]].SkillLevel + players[indices[1]].SkillLevel;
            var team2Total = players[indices[2]].SkillLevel + players[indices[3]].SkillLevel;
            var difference = Math.Abs(team1Total - team2Total);

            if (difference < minDifference)
            {
                minDifference = difference;
                bestOrder = indices.Select(i => players[i]).ToList();
            }
        }

        return (minDifference, bestOrder);
    }
}
