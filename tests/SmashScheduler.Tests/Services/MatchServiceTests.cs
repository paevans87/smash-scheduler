using FluentAssertions;
using Moq;
using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.MatchManagement;
using SmashScheduler.Domain.Enums;
using DomainMatch = SmashScheduler.Domain.Entities.Match;
using Xunit;

namespace SmashScheduler.Tests.Services;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock;
    private readonly MatchService _matchService;

    public MatchServiceTests()
    {
        _matchRepositoryMock = new Mock<IMatchRepository>();
        _matchService = new MatchService(_matchRepositoryMock.Object);
    }

    [Fact]
    public async Task UpdateMatchPlayersAsync_WithValidData_UpdatesPlayersAndSetsManualFlag()
    {
        var matchId = Guid.NewGuid();
        var originalPlayerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var newPlayerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        var existingMatch = new DomainMatch
        {
            Id = matchId,
            SessionId = Guid.NewGuid(),
            CourtNumber = 1,
            PlayerIds = originalPlayerIds,
            State = MatchState.InProgress,
            WasAutomated = true
        };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync(existingMatch);

        await _matchService.UpdateMatchPlayersAsync(matchId, newPlayerIds);

        existingMatch.PlayerIds.Should().BeEquivalentTo(newPlayerIds);
        existingMatch.WasAutomated.Should().BeFalse();
        _matchRepositoryMock.Verify(r => r.UpdateAsync(existingMatch), Times.Once);
    }

    [Fact]
    public async Task UpdateMatchPlayersAsync_WithNonExistentMatch_ThrowsException()
    {
        var matchId = Guid.NewGuid();
        var newPlayerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync((DomainMatch?)null);

        var act = async () => await _matchService.UpdateMatchPlayersAsync(matchId, newPlayerIds);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Match not found");
    }

    [Fact]
    public async Task UpdateMatchPlayersAsync_PreservesOtherMatchProperties()
    {
        var matchId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var startedAt = DateTime.UtcNow.AddMinutes(-10);
        var originalPlayerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var newPlayerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        var existingMatch = new DomainMatch
        {
            Id = matchId,
            SessionId = sessionId,
            CourtNumber = 2,
            PlayerIds = originalPlayerIds,
            State = MatchState.InProgress,
            WasAutomated = true,
            StartedAt = startedAt
        };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId)).ReturnsAsync(existingMatch);

        await _matchService.UpdateMatchPlayersAsync(matchId, newPlayerIds);

        existingMatch.Id.Should().Be(matchId);
        existingMatch.SessionId.Should().Be(sessionId);
        existingMatch.CourtNumber.Should().Be(2);
        existingMatch.State.Should().Be(MatchState.InProgress);
        existingMatch.StartedAt.Should().Be(startedAt);
    }

    [Fact]
    public async Task CreateMatchAsync_WithValidData_CreatesMatch()
    {
        var sessionId = Guid.NewGuid();
        var courtNumber = 1;
        var playerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var wasAutomated = true;

        var result = await _matchService.CreateMatchAsync(sessionId, courtNumber, playerIds, wasAutomated);

        result.Should().NotBeNull();
        result.SessionId.Should().Be(sessionId);
        result.CourtNumber.Should().Be(courtNumber);
        result.PlayerIds.Should().BeEquivalentTo(playerIds);
        result.WasAutomated.Should().Be(wasAutomated);
        result.State.Should().Be(MatchState.InProgress);
        _matchRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<DomainMatch>()), Times.Once);
    }

    [Fact]
    public async Task CreateMatchAsync_WithWasAutomatedFalse_CreatesManualMatch()
    {
        var sessionId = Guid.NewGuid();
        var courtNumber = 1;
        var playerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var wasAutomated = false;

        var result = await _matchService.CreateMatchAsync(sessionId, courtNumber, playerIds, wasAutomated);

        result.WasAutomated.Should().BeFalse();
    }
}
