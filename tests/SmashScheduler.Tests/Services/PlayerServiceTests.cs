using FluentAssertions;
using Moq;
using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Application.Services.PlayerManagement;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
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
    public async Task GetByIdAsync_WithExistingPlayer_ReturnsPlayer()
    {
        var playerId = Guid.NewGuid();
        var expectedPlayer = new Player { Id = playerId, Name = "Test" };
        _playerRepositoryMock.Setup(r => r.GetByIdAsync(playerId)).ReturnsAsync(expectedPlayer);

        var result = await _playerService.GetByIdAsync(playerId);

        result.Should().Be(expectedPlayer);
    }

    [Fact]
    public async Task GetByClubIdAsync_ReturnsPlayersForClub()
    {
        var clubId = Guid.NewGuid();
        var expectedPlayers = new List<Player>
        {
            new Player { Id = Guid.NewGuid(), ClubId = clubId, Name = "Player 1" },
            new Player { Id = Guid.NewGuid(), ClubId = clubId, Name = "Player 2" }
        };
        _playerRepositoryMock.Setup(r => r.GetByClubIdAsync(clubId)).ReturnsAsync(expectedPlayers);

        var result = await _playerService.GetByClubIdAsync(clubId);

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedPlayers);
    }

    [Fact]
    public async Task DeletePlayerAsync_CallsRepository()
    {
        var playerId = Guid.NewGuid();

        await _playerService.DeletePlayerAsync(playerId);

        _playerRepositoryMock.Verify(r => r.DeleteAsync(playerId), Times.Once);
    }
}
