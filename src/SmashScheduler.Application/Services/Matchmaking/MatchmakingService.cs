using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.Matchmaking.Models;
using SmashScheduler.Application.Services.Matchmaking.ScoringStrategies;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
using SmashScheduler.Domain.ValueObjects;

namespace SmashScheduler.Application.Services.Matchmaking;

public class MatchmakingService(
    ISessionRepository sessionRepository,
    IMatchRepository matchRepository,
    IPlayerRepository playerRepository,
    IClubRepository clubRepository) : IMatchmakingService
{
    public async Task<List<MatchCandidate>> GenerateMatchesAsync(Guid sessionId)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId);
        if (session == null) throw new InvalidOperationException("Session not found");

        var club = await clubRepository.GetByIdAsync(session.ClubId);
        var weights = club?.ScoringWeights ?? new ScoringWeights();
        var blacklistMode = club?.BlacklistMode ?? BlacklistMode.Preferred;

        var existingMatches = await matchRepository.GetBySessionIdAsync(sessionId);

        var playingPlayerIds = existingMatches
            .Where(m => m.State == MatchState.InProgress)
            .SelectMany(m => m.PlayerIds)
            .ToHashSet();

        var benchedSessionPlayers = session.SessionPlayers
            .Where(sp => sp.IsActive && !playingPlayerIds.Contains(sp.PlayerId))
            .ToList();

        var benchedPlayers = new List<Player>();
        foreach (var sp in benchedSessionPlayers)
        {
            var player = sp.Player ?? await playerRepository.GetByIdAsync(sp.PlayerId);
            if (player != null)
            {
                benchedPlayers.Add(player);
            }
        }

        var blacklists = await LoadBlacklistsForPlayers(benchedPlayers);

        var usedCourts = existingMatches
            .Where(m => m.State == MatchState.InProgress)
            .Select(m => m.CourtNumber)
            .ToHashSet();

        var availableCourts = Enumerable.Range(1, session.CourtCount)
            .Where(c => !usedCourts.Contains(c))
            .ToList();

        var completedMatches = existingMatches.Where(m => m.State == MatchState.Completed).ToList();
        var lastMatchTimes = BuildLastMatchCompletionTimes(completedMatches);

        return GenerateScoredMatches(benchedPlayers, availableCourts, completedMatches, lastMatchTimes, weights, blacklistMode, blacklists);
    }

    public async Task<MatchCandidate?> GenerateSingleMatchAsync(Guid sessionId, int courtNumber)
    {
        var session = await sessionRepository.GetByIdAsync(sessionId);
        if (session == null) throw new InvalidOperationException("Session not found");

        var club = await clubRepository.GetByIdAsync(session.ClubId);
        var weights = club?.ScoringWeights ?? new ScoringWeights();
        var blacklistMode = club?.BlacklistMode ?? BlacklistMode.Preferred;

        var existingMatches = await matchRepository.GetBySessionIdAsync(sessionId);

        var playingPlayerIds = existingMatches
            .Where(m => m.State == MatchState.InProgress)
            .SelectMany(m => m.PlayerIds)
            .ToHashSet();

        var benchedSessionPlayers = session.SessionPlayers
            .Where(sp => sp.IsActive && !playingPlayerIds.Contains(sp.PlayerId))
            .ToList();

        var benchedPlayers = new List<Player>();
        foreach (var sp in benchedSessionPlayers)
        {
            var player = sp.Player ?? await playerRepository.GetByIdAsync(sp.PlayerId);
            if (player != null)
            {
                benchedPlayers.Add(player);
            }
        }

        if (benchedPlayers.Count < 4)
        {
            return null;
        }

        var blacklists = await LoadBlacklistsForPlayers(benchedPlayers);
        var completedMatches = existingMatches.Where(m => m.State == MatchState.Completed).ToList();
        var lastMatchTimes = BuildLastMatchCompletionTimes(completedMatches);
        var context = new MatchScoringContext
        {
            CompletedMatches = completedMatches,
            LastMatchCompletionTimes = lastMatchTimes
        };

        var bestCandidate = FindBestFoursomeWithScoring(benchedPlayers, context, weights, blacklistMode, blacklists);
        if (bestCandidate == null) return null;

        bestCandidate.CourtNumber = courtNumber;
        return bestCandidate;
    }

    private List<MatchCandidate> GenerateScoredMatches(
        List<Player> players,
        List<int> availableCourts,
        List<Match> completedMatches,
        Dictionary<Guid, DateTime> lastMatchTimes,
        ScoringWeights weights,
        BlacklistMode blacklistMode,
        List<PlayerBlacklist> blacklists)
    {
        var candidates = new List<MatchCandidate>();

        if (players.Count < 4 || !availableCourts.Any())
        {
            return candidates;
        }

        var context = new MatchScoringContext
        {
            CompletedMatches = completedMatches,
            LastMatchCompletionTimes = lastMatchTimes
        };

        var remainingPlayers = new List<Player>(players);
        var courtIndex = 0;

        while (remainingPlayers.Count >= 4 && courtIndex < availableCourts.Count)
        {
            var bestCandidate = FindBestFoursomeWithScoring(remainingPlayers, context, weights, blacklistMode, blacklists);
            if (bestCandidate == null) break;

            bestCandidate.CourtNumber = availableCourts[courtIndex];
            candidates.Add(bestCandidate);

            remainingPlayers = remainingPlayers
                .Where(p => !bestCandidate.PlayerIds.Contains(p.Id))
                .ToList();

            courtIndex++;
        }

        return candidates;
    }

    private MatchCandidate? FindBestFoursomeWithScoring(
        List<Player> availablePlayers,
        MatchScoringContext context,
        ScoringWeights weights,
        BlacklistMode blacklistMode,
        List<PlayerBlacklist> blacklists)
    {
        if (availablePlayers.Count < 4)
        {
            return null;
        }

        var allCombinations = GenerateFoursomeCombinations(availablePlayers);
        var skillScorer = new SkillBalanceScorer();
        var historyScorer = new MatchHistoryScorer();
        var timeScorer = new TimeOffCourtScorer();
        var blacklistScorer = new BlacklistAvoidanceScorer(blacklists);

        var skillWeight = weights.SkillBalance / 100.0;
        var historyWeight = weights.MatchHistory / 100.0;
        var timeWeight = weights.TimeOffCourt / 100.0;

        MatchCandidate? bestCandidate = null;
        var bestScore = double.MinValue;

        foreach (var combination in allCombinations)
        {
            var candidate = new MatchCandidate
            {
                PlayerIds = combination.Select(p => p.Id).ToList()
            };

            if (blacklistMode == BlacklistMode.HardLimit && HasBlacklistViolation(candidate.PlayerIds, blacklists))
            {
                continue;
            }

            var skillScore = skillScorer.CalculateScore(candidate, availablePlayers, context);
            var historyScore = historyScorer.CalculateScore(candidate, availablePlayers, context);
            var timeScore = timeScorer.CalculateScore(candidate, availablePlayers, context);

            var totalScore = (skillScore * skillWeight) +
                             (historyScore * historyWeight) +
                             (timeScore * timeWeight);

            if (blacklistMode == BlacklistMode.Preferred && blacklists.Any())
            {
                var blacklistScore = blacklistScorer.CalculateScore(candidate, availablePlayers, context);
                totalScore *= (blacklistScore / 100.0);
            }

            candidate.TotalScore = totalScore;

            if (totalScore > bestScore)
            {
                bestScore = totalScore;
                bestCandidate = candidate;
            }
        }

        return bestCandidate;
    }

    private static bool HasBlacklistViolation(List<Guid> playerIds, List<PlayerBlacklist> blacklists)
    {
        foreach (var playerId in playerIds)
        {
            foreach (var blacklist in blacklists.Where(b => b.PlayerId == playerId))
            {
                if (playerIds.Contains(blacklist.BlacklistedPlayerId))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private async Task<List<PlayerBlacklist>> LoadBlacklistsForPlayers(List<Player> players)
    {
        var allBlacklists = new List<PlayerBlacklist>();
        foreach (var player in players)
        {
            var blacklists = await playerRepository.GetBlacklistsByPlayerIdAsync(player.Id);
            allBlacklists.AddRange(blacklists);
        }

        return allBlacklists;
    }

    private List<List<Player>> GenerateFoursomeCombinations(List<Player> players)
    {
        var combinations = new List<List<Player>>();
        var count = players.Count;

        if (count > 12)
        {
            var sorted = players.OrderByDescending(p => p.SkillLevel).ToList();
            return GenerateLimitedCombinations(sorted, 100);
        }

        for (var i = 0; i < count - 3; i++)
        {
            for (var j = i + 1; j < count - 2; j++)
            {
                for (var k = j + 1; k < count - 1; k++)
                {
                    for (var l = k + 1; l < count; l++)
                    {
                        combinations.Add(new List<Player>
                        {
                            players[i], players[j], players[k], players[l]
                        });
                    }
                }
            }
        }

        return combinations;
    }

    private List<List<Player>> GenerateLimitedCombinations(List<Player> sortedPlayers, int maxCombinations)
    {
        var combinations = new List<List<Player>>();
        var count = sortedPlayers.Count;
        var random = new Random();

        for (var attempt = 0; attempt < maxCombinations && combinations.Count < maxCombinations; attempt++)
        {
            var indices = Enumerable.Range(0, count).OrderBy(_ => random.Next()).Take(4).OrderBy(x => x).ToList();
            var combination = indices.Select(idx => sortedPlayers[idx]).ToList();

            var alreadyExists = combinations.Any(c =>
                c.Select(p => p.Id).OrderBy(id => id).SequenceEqual(combination.Select(p => p.Id).OrderBy(id => id)));

            if (!alreadyExists)
            {
                combinations.Add(combination);
            }
        }

        return combinations;
    }

    private Dictionary<Guid, DateTime> BuildLastMatchCompletionTimes(List<Match> completedMatches)
    {
        var lastMatchTimes = new Dictionary<Guid, DateTime>();

        foreach (var match in completedMatches.Where(m => m.CompletedAt.HasValue).OrderBy(m => m.CompletedAt))
        {
            foreach (var playerId in match.PlayerIds)
            {
                lastMatchTimes[playerId] = match.CompletedAt!.Value;
            }
        }

        return lastMatchTimes;
    }
}
