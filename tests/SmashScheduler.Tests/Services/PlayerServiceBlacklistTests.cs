using FluentAssertions;
using Moq;
using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.PlayerManagement;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
using Xunit;

namespace SmashScheduler.Tests.Services;

public class PlayerServiceBlacklistTests
{
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly PlayerService _playerService;

    public PlayerServiceBlacklistTests()
    {
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _playerService = new PlayerService(_playerRepositoryMock.Object, new HttpClient());
    }

    [Fact]
    public async Task AddToBlacklistAsync_WithSelfBlacklist_DoesNotAdd()
    {
        var playerId = Guid.NewGuid();
        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(new List<PlayerBlacklist>());

        await _playerService.AddToBlacklistAsync(playerId, playerId, BlacklistType.Partner);

        _playerRepositoryMock.Verify(r => r.AddToBlacklistAsync(It.IsAny<PlayerBlacklist>()), Times.Never);
    }

    [Fact]
    public async Task AddToBlacklistAsync_WithDuplicate_DoesNotAdd()
    {
        var playerId = Guid.NewGuid();
        var blacklistedPlayerId = Guid.NewGuid();
        var existingBlacklist = new PlayerBlacklist
        {
            PlayerId = playerId,
            BlacklistedPlayerId = blacklistedPlayerId,
            BlacklistType = BlacklistType.Partner
        };
        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(new List<PlayerBlacklist> { existingBlacklist });

        await _playerService.AddToBlacklistAsync(playerId, blacklistedPlayerId, BlacklistType.Partner);

        _playerRepositoryMock.Verify(r => r.AddToBlacklistAsync(It.IsAny<PlayerBlacklist>()), Times.Never);
    }

    [Fact]
    public async Task AddToBlacklistAsync_WithDifferentBlacklistType_AddsSuccessfully()
    {
        var playerId = Guid.NewGuid();
        var blacklistedPlayerId = Guid.NewGuid();
        var existingBlacklist = new PlayerBlacklist
        {
            PlayerId = playerId,
            BlacklistedPlayerId = blacklistedPlayerId,
            BlacklistType = BlacklistType.Partner
        };
        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(new List<PlayerBlacklist> { existingBlacklist });

        await _playerService.AddToBlacklistAsync(playerId, blacklistedPlayerId, BlacklistType.Opponent);

        _playerRepositoryMock.Verify(r => r.AddToBlacklistAsync(It.Is<PlayerBlacklist>(b =>
            b.PlayerId == playerId &&
            b.BlacklistedPlayerId == blacklistedPlayerId &&
            b.BlacklistType == BlacklistType.Opponent)), Times.Once);
    }

    [Fact]
    public async Task AddToBlacklistAsync_WithNewEntry_AddsSuccessfully()
    {
        var playerId = Guid.NewGuid();
        var blacklistedPlayerId = Guid.NewGuid();
        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(new List<PlayerBlacklist>());

        await _playerService.AddToBlacklistAsync(playerId, blacklistedPlayerId, BlacklistType.Partner);

        _playerRepositoryMock.Verify(r => r.AddToBlacklistAsync(It.Is<PlayerBlacklist>(b =>
            b.PlayerId == playerId &&
            b.BlacklistedPlayerId == blacklistedPlayerId &&
            b.BlacklistType == BlacklistType.Partner)), Times.Once);
    }

    [Fact]
    public async Task RemoveFromBlacklistAsync_CallsRepository()
    {
        var playerId = Guid.NewGuid();
        var blacklistedPlayerId = Guid.NewGuid();

        await _playerService.RemoveFromBlacklistAsync(playerId, blacklistedPlayerId, BlacklistType.Partner);

        _playerRepositoryMock.Verify(r => r.RemoveFromBlacklistAsync(playerId, blacklistedPlayerId), Times.Once);
    }

    [Fact]
    public async Task GetBlacklistsAsync_ReturnsBlacklistsFromRepository()
    {
        var playerId = Guid.NewGuid();
        var expectedBlacklists = new List<PlayerBlacklist>
        {
            new PlayerBlacklist { PlayerId = playerId, BlacklistedPlayerId = Guid.NewGuid(), BlacklistType = BlacklistType.Partner },
            new PlayerBlacklist { PlayerId = playerId, BlacklistedPlayerId = Guid.NewGuid(), BlacklistType = BlacklistType.Opponent }
        };
        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(expectedBlacklists);

        var result = await _playerService.GetBlacklistsAsync(playerId);

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedBlacklists);
    }

    [Fact]
    public async Task GetBlacklistsAsync_WithNoBlacklists_ReturnsEmptyList()
    {
        var playerId = Guid.NewGuid();
        _playerRepositoryMock.Setup(r => r.GetBlacklistsByPlayerIdAsync(playerId))
            .ReturnsAsync(new List<PlayerBlacklist>());

        var result = await _playerService.GetBlacklistsAsync(playerId);

        result.Should().BeEmpty();
    }
}
