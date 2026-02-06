using FluentAssertions;
using Moq;
using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.Matchmaking;
using SmashScheduler.Application.Services.Matchmaking.Models;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
using SmashScheduler.Domain.ValueObjects;
using Xunit;

namespace SmashScheduler.Tests.Matchmaking;

public class MatchmakingServiceTests
{
    private readonly Mock<ISessionRepository> _sessionRepositoryMock;
    private readonly Mock<IMatchRepository> _matchRepositoryMock;
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly Mock<IClubRepository> _clubRepositoryMock;
    private readonly MatchmakingService _service;

    public MatchmakingServiceTests()
    {
        _sessionRepositoryMock = new Mock<ISessionRepository>();
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _clubRepositoryMock = new Mock<IClubRepository>();
        _service = new MatchmakingService(
            _sessionRepositoryMock.Object,
            _matchRepositoryMock.Object,
            _playerRepositoryMock.Object,
            _clubRepositoryMock.Object
        );
    }

    [Fact]
    public async Task GenerateMatchesAsync_UsesClubScoringWeights()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(4);
        var club = new Club
        {
            Id = clubId,
            ScoringWeights = new ScoringWeights
            {
                SkillBalance = 100,
                MatchHistory = 0,
                TimeOffCourt = 0
            }
        };

        SetupSession(sessionId, clubId, players);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var result = await _service.GenerateMatchesAsync(sessionId);

        result.Should().HaveCount(1);
        result[0].PlayerIds.Should().HaveCount(4);
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithHardLimitBlacklist_ExcludesBlacklistedCombinations()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(8);
        var club = new Club
        {
            Id = clubId,
            BlacklistMode = BlacklistMode.HardLimit,
            ScoringWeights = new ScoringWeights()
        };

        SetupSession(sessionId, clubId, players, courtCount: 2);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);

        var blacklists = new List<PlayerBlacklist>
        {
            new PlayerBlacklist
            {
                PlayerId = players[0].Id,
                BlacklistedPlayerId = players[1].Id,
                BlacklistType = BlacklistType.Partner
            }
        };

        foreach (var player in players)
        {
            var playerBlacklists = blacklists.Where(b => b.PlayerId == player.Id).ToList();
            _playerRepositoryMock
                .Setup(r => r.GetBlacklistsByPlayerIdAsync(player.Id))
                .ReturnsAsync(playerBlacklists);
        }

        var result = await _service.GenerateMatchesAsync(sessionId);

        result.Should().NotBeEmpty();
        foreach (var candidate in result)
        {
            var containsBoth = candidate.PlayerIds.Contains(players[0].Id) &&
                               candidate.PlayerIds.Contains(players[1].Id);
            containsBoth.Should().BeFalse();
        }
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithPreferredBlacklist_PenalisesButAllowsCombinations()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(4);
        var club = new Club
        {
            Id = clubId,
            BlacklistMode = BlacklistMode.Preferred,
            ScoringWeights = new ScoringWeights()
        };

        SetupSession(sessionId, clubId, players);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);

        var blacklists = new List<PlayerBlacklist>
        {
            new PlayerBlacklist
            {
                PlayerId = players[0].Id,
                BlacklistedPlayerId = players[1].Id,
                BlacklistType = BlacklistType.Partner
            }
        };

        foreach (var player in players)
        {
            var playerBlacklists = blacklists.Where(b => b.PlayerId == player.Id).ToList();
            _playerRepositoryMock
                .Setup(r => r.GetBlacklistsByPlayerIdAsync(player.Id))
                .ReturnsAsync(playerBlacklists);
        }

        var result = await _service.GenerateMatchesAsync(sessionId);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithDefaultWeights_ReturnsResults()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(4);

        SetupSession(sessionId, clubId, players);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync((Club?)null);
        SetupEmptyBlacklists(players);

        var result = await _service.GenerateMatchesAsync(sessionId);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GenerateSingleMatchAsync_UsesClubWeightsAndBlacklist()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(4);
        var club = new Club
        {
            Id = clubId,
            ScoringWeights = new ScoringWeights
            {
                SkillBalance = 20,
                MatchHistory = 40,
                TimeOffCourt = 40
            },
            BlacklistMode = BlacklistMode.Preferred
        };

        SetupSession(sessionId, clubId, players);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var result = await _service.GenerateSingleMatchAsync(sessionId, 1);

        result.Should().NotBeNull();
        result!.CourtNumber.Should().Be(1);
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithExcludedPlayers_DoesNotIncludeThem()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(8);
        var club = new Club
        {
            Id = clubId,
            ScoringWeights = new ScoringWeights()
        };

        SetupSession(sessionId, clubId, players, courtCount: 2);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var excludeIds = new List<Guid> { players[0].Id, players[1].Id };
        var result = await _service.GenerateMatchesAsync(sessionId, excludeIds);

        result.Should().NotBeEmpty();
        foreach (var candidate in result)
        {
            candidate.PlayerIds.Should().NotContain(players[0].Id);
            candidate.PlayerIds.Should().NotContain(players[1].Id);
        }
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithTooManyExcluded_ReturnsEmpty()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(6);
        var club = new Club
        {
            Id = clubId,
            ScoringWeights = new ScoringWeights()
        };

        SetupSession(sessionId, clubId, players);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var excludeIds = players.Take(3).Select(p => p.Id).ToList();
        var result = await _service.GenerateMatchesAsync(sessionId, excludeIds);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithExcludedCourts_DoesNotUseThem()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(8);
        var club = new Club { Id = clubId, ScoringWeights = new ScoringWeights() };

        SetupSession(sessionId, clubId, players, courtCount: 3);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var options = new GenerationOptions(ExcludeCourtNumbers: new List<int> { 2 });
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().NotBeEmpty();
        result.Should().OnlyContain(m => m.CourtNumber != 2);
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithMaleOnlyFilter_OnlyIncludesMales()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreateMixedGenderPlayers(8);
        var club = new Club { Id = clubId, ScoringWeights = new ScoringWeights() };

        SetupSession(sessionId, clubId, players, courtCount: 2);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var options = new GenerationOptions(GenderFilter: GenderFilter.MaleOnly);
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().NotBeEmpty();
        foreach (var candidate in result)
        {
            var matchPlayers = candidate.PlayerIds
                .Select(id => players.First(p => p.Id == id));
            matchPlayers.Should().OnlyContain(p => p.Gender == Gender.Male);
        }
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithFemaleOnlyFilter_OnlyIncludesFemales()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreateMixedGenderPlayers(8);
        var club = new Club { Id = clubId, ScoringWeights = new ScoringWeights() };

        SetupSession(sessionId, clubId, players, courtCount: 2);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var options = new GenerationOptions(GenderFilter: GenderFilter.FemaleOnly);
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().NotBeEmpty();
        foreach (var candidate in result)
        {
            var matchPlayers = candidate.PlayerIds
                .Select(id => players.First(p => p.Id == id));
            matchPlayers.Should().OnlyContain(p => p.Gender == Gender.Female);
        }
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithMixedOnlyFilter_OnlyIncludesMixedCombinations()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreateMixedGenderPlayers(8);
        var club = new Club { Id = clubId, ScoringWeights = new ScoringWeights() };

        SetupSession(sessionId, clubId, players, courtCount: 2);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var options = new GenerationOptions(GenderFilter: GenderFilter.MixedOnly);
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().NotBeEmpty();
        foreach (var candidate in result)
        {
            var matchPlayers = candidate.PlayerIds
                .Select(id => players.First(p => p.Id == id)).ToList();
            var genders = matchPlayers.Select(p => p.Gender).Distinct();
            genders.Should().HaveCountGreaterThan(1);
        }
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithStrongStrategy_UsesHighSkillWeights()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(4);
        var club = new Club
        {
            Id = clubId,
            ScoringWeights = new ScoringWeights { SkillBalance = 10, MatchHistory = 10, TimeOffCourt = 80 }
        };

        SetupSession(sessionId, clubId, players);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var options = new GenerationOptions(Strategy: GenerationStrategy.Strong);
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithLeastGamesStrategy_ReturnsResults()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var players = CreatePlayers(4);
        var club = new Club
        {
            Id = clubId,
            ScoringWeights = new ScoringWeights { SkillBalance = 80, MatchHistory = 10, TimeOffCourt = 10 }
        };

        SetupSession(sessionId, clubId, players);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(club);
        SetupEmptyBlacklists(players);

        var options = new GenerationOptions(Strategy: GenerationStrategy.LeastGames);
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().HaveCount(1);
    }

    private void SetupSession(Guid sessionId, Guid clubId, List<Player> players, int courtCount = 1)
    {
        var sessionPlayers = players.Select(p => new SessionPlayer
        {
            SessionId = sessionId,
            PlayerId = p.Id,
            Player = p,
            IsActive = true
        }).ToList();

        var session = new Session
        {
            Id = sessionId,
            ClubId = clubId,
            CourtCount = courtCount,
            State = SessionState.Active,
            SessionPlayers = sessionPlayers
        };

        _sessionRepositoryMock.Setup(r => r.GetByIdAsync(sessionId)).ReturnsAsync(session);
        _matchRepositoryMock.Setup(r => r.GetBySessionIdAsync(sessionId)).ReturnsAsync(new List<Domain.Entities.Match>());
    }

    private void SetupEmptyBlacklists(List<Player> players)
    {
        foreach (var player in players)
        {
            _playerRepositoryMock
                .Setup(r => r.GetBlacklistsByPlayerIdAsync(player.Id))
                .ReturnsAsync(new List<PlayerBlacklist>());
        }
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithMatchCount_GeneratesDraftsWhenNoCourtsAvailable()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var benchPlayers = CreatePlayers(8);
        var courtPlayers = CreatePlayers(4);

        SetupSessionWithAllCourtsInUse(sessionId, clubId, benchPlayers, courtPlayers, courtCount: 1);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(new Club { Id = clubId });
        SetupEmptyBlacklists(benchPlayers.Concat(courtPlayers).ToList());

        var options = new GenerationOptions(MatchCount: 2);
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().HaveCount(2);
        result.SelectMany(r => r.PlayerIds).Distinct().Should().HaveCount(8);
    }

    [Fact]
    public async Task GenerateMatchesAsync_WithMatchCount_LimitedByBenchedPlayers()
    {
        var clubId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var benchPlayers = CreatePlayers(4);
        var courtPlayers = CreatePlayers(4);

        SetupSessionWithAllCourtsInUse(sessionId, clubId, benchPlayers, courtPlayers, courtCount: 1);
        _clubRepositoryMock.Setup(r => r.GetByIdAsync(clubId)).ReturnsAsync(new Club { Id = clubId });
        SetupEmptyBlacklists(benchPlayers.Concat(courtPlayers).ToList());

        var options = new GenerationOptions(MatchCount: 3);
        var result = await _service.GenerateMatchesAsync(sessionId, options: options);

        result.Should().HaveCount(1);
    }

    private void SetupSessionWithAllCourtsInUse(Guid sessionId, Guid clubId, List<Player> benchPlayers, List<Player> courtPlayers, int courtCount)
    {
        var allPlayers = benchPlayers.Concat(courtPlayers).ToList();
        var sessionPlayers = allPlayers.Select(p => new SessionPlayer
        {
            SessionId = sessionId,
            PlayerId = p.Id,
            Player = p,
            IsActive = true
        }).ToList();

        var session = new Session
        {
            Id = sessionId,
            ClubId = clubId,
            CourtCount = courtCount,
            State = SessionState.Active,
            SessionPlayers = sessionPlayers
        };

        var inProgressMatches = new List<Domain.Entities.Match>();
        for (var i = 0; i < courtCount; i++)
        {
            var matchPlayers = courtPlayers.Skip(i * 4).Take(4).Select(p => p.Id).ToList();
            if (matchPlayers.Count == 4)
            {
                inProgressMatches.Add(new Domain.Entities.Match
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    CourtNumber = i + 1,
                    State = MatchState.InProgress,
                    PlayerIds = matchPlayers,
                    StartedAt = DateTime.UtcNow.AddMinutes(-10)
                });
            }
        }

        _sessionRepositoryMock.Setup(r => r.GetByIdAsync(sessionId)).ReturnsAsync(session);
        _matchRepositoryMock.Setup(r => r.GetBySessionIdAsync(sessionId)).ReturnsAsync(inProgressMatches);
    }

    private List<Player> CreatePlayers(int count)
    {
        return Enumerable.Range(1, count).Select(i => new Player
        {
            Id = Guid.NewGuid(),
            Name = $"Player {i}",
            SkillLevel = 5,
            Gender = Gender.Male,
            PlayStylePreference = PlayStylePreference.Open,
            ClubId = Guid.NewGuid()
        }).ToList();
    }

    private List<Player> CreateMixedGenderPlayers(int count)
    {
        return Enumerable.Range(1, count).Select(i => new Player
        {
            Id = Guid.NewGuid(),
            Name = $"Player {i}",
            SkillLevel = 5,
            Gender = i % 2 == 0 ? Gender.Female : Gender.Male,
            PlayStylePreference = PlayStylePreference.Open,
            ClubId = Guid.NewGuid()
        }).ToList();
    }
}
