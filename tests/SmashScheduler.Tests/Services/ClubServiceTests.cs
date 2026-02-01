using FluentAssertions;
using Moq;
using SmashScheduler.Application.Services.ClubManagement;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
using SmashScheduler.Infrastructure.Data.Repositories;
using Xunit;

namespace SmashScheduler.Tests.Services;

public class ClubServiceTests
{
    private readonly Mock<IClubRepository> _clubRepositoryMock;
    private readonly Mock<IPlayerRepository> _playerRepositoryMock;
    private readonly Mock<ISessionRepository> _sessionRepositoryMock;
    private readonly ClubService _clubService;

    public ClubServiceTests()
    {
        _clubRepositoryMock = new Mock<IClubRepository>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _sessionRepositoryMock = new Mock<ISessionRepository>();
        _clubService = new ClubService(
            _clubRepositoryMock.Object,
            _playerRepositoryMock.Object,
            _sessionRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateClubAsync_WithValidData_CreatesClub()
    {
        var name = "Test Club";
        var courtCount = 4;
        var gameType = GameType.Doubles;

        var result = await _clubService.CreateClubAsync(name, courtCount, gameType);

        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.DefaultCourtCount.Should().Be(courtCount);
        result.GameType.Should().Be(gameType);
        _clubRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Club>()), Times.Once);
    }

    [Fact]
    public async Task GetAllClubsAsync_ReturnsAllClubs()
    {
        var expectedClubs = new List<Club>
        {
            new Club { Id = Guid.NewGuid(), Name = "Club 1" },
            new Club { Id = Guid.NewGuid(), Name = "Club 2" }
        };
        _clubRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedClubs);

        var result = await _clubService.GetAllClubsAsync();

        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedClubs);
    }

    [Fact]
    public async Task DeleteClubAsync_DeletesClubAndRelatedData()
    {
        var clubId = Guid.NewGuid();
        var players = new List<Player>
        {
            new Player { Id = Guid.NewGuid(), ClubId = clubId }
        };
        var sessions = new List<Session>
        {
            new Session { Id = Guid.NewGuid(), ClubId = clubId }
        };

        _playerRepositoryMock.Setup(r => r.GetByClubIdAsync(clubId)).ReturnsAsync(players);
        _sessionRepositoryMock.Setup(r => r.GetByClubIdAsync(clubId)).ReturnsAsync(sessions);

        await _clubService.DeleteClubAsync(clubId);

        _playerRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        _sessionRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        _clubRepositoryMock.Verify(r => r.DeleteAsync(clubId), Times.Once);
    }
}
