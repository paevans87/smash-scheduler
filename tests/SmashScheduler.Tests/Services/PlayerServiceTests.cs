using FluentAssertions;
using Moq;
using SmashScheduler.Application.Services.PlayerManagement;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
using SmashScheduler.Infrastructure.Data.Repositories;
using Xunit;

namespace SmashScheduler.Tests.Services;

public class PlayerServiceTests
{
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly PlayerService _playerService;

    public PlayerServiceTests()
    {
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _playerService = new PlayerService(_playerRepositoryMock.Object);
    }

    [Fact]
    public async Task CreatePlayerAsync_WithValidData_CreatesPlayer()
    {
        var clubId = Guid.NewGuid();
        var name = "Test Player";
        var skillLevel = 7;
        var gender = Gender.Female;
        var playStyle = PlayStylePreference.Level;

        var result = await _playerService.CreatePlayerAsync(clubId, name, skillLevel, gender, playStyle);

        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.SkillLevel.Should().Be(skillLevel);
        result.Gender.Should().Be(gender);
        result.PlayStylePreference.Should().Be(playStyle);
        _playerRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Player>()), Times.Once);
    }

    [Fact]
    public async Task CreatePlayerAsync_WithInvalidSkillLevel_ThrowsException()
    {
        var clubId = Guid.NewGuid();

        var act = async () => await _playerService.CreatePlayerAsync(clubId, "Test", 11, Gender.Male, PlayStylePreference.Open);

        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddToBlacklistAsync_PreventsDuplicates()
    {
        var playerId = Guid.NewGuid();
        var blacklistedId = Guid.NewGuid();
        var existingBlacklists = new List<PlayerBlacklist>
        {
            new PlayerBlacklist
            {
                PlayerId = playerId,
                BlacklistedPlayerId = blacklistedId,
                BlacklistType = BlacklistType.Partner
            }
        };

        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(existingBlacklists);

        await _playerService.AddToBlacklistAsync(playerId, blacklistedId, BlacklistType.Partner);

        _playerRepositoryMock.Verify(r => r.AddToBlacklistAsync(It.IsAny<PlayerBlacklist>()), Times.Never);
    }

    [Fact]
    public async Task AddToBlacklistAsync_WithNewEntry_AddsToBlacklist()
    {
        var playerId = Guid.NewGuid();
        var blacklistedId = Guid.NewGuid();

        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(new List<PlayerBlacklist>());

        await _playerService.AddToBlacklistAsync(playerId, blacklistedId, BlacklistType.Opponent);

        _playerRepositoryMock.Verify(r => r.AddToBlacklistAsync(It.IsAny<PlayerBlacklist>()), Times.Once);
    }
}
