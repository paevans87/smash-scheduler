using FluentAssertions;
using Moq;
using SmashScheduler.Application.Services.SessionManagement;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;
using SmashScheduler.Infrastructure.Data.Repositories;
using Xunit;

namespace SmashScheduler.Tests.Services;

public class SessionStateManagerTests
{
    private readonly Mock<ISessionRepository> _sessionRepositoryMock;
    private readonly SessionStateManager _sessionStateManager;

    public SessionStateManagerTests()
    {
        _sessionRepositoryMock = new Mock<ISessionRepository>();
        _sessionStateManager = new SessionStateManager(_sessionRepositoryMock.Object);
    }

    [Fact]
    public async Task ActivateSessionAsync_WithDraftSession_ActivatesSession()
    {
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            State = SessionState.Draft
        };

        _sessionRepositoryMock.Setup(r => r.GetByIdAsync(sessionId)).ReturnsAsync(session);

        await _sessionStateManager.ActivateSessionAsync(sessionId);

        session.State.Should().Be(SessionState.Active);
        _sessionRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Session>(s => s.State == SessionState.Active)), Times.Once);
    }

    [Fact]
    public async Task ActivateSessionAsync_WithNonDraftSession_ThrowsException()
    {
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            State = SessionState.Active
        };

        _sessionRepositoryMock.Setup(r => r.GetByIdAsync(sessionId)).ReturnsAsync(session);

        var act = async () => await _sessionStateManager.ActivateSessionAsync(sessionId);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CompleteSessionAsync_WithActiveSession_CompletesSession()
    {
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            State = SessionState.Active
        };

        _sessionRepositoryMock.Setup(r => r.GetByIdAsync(sessionId)).ReturnsAsync(session);

        await _sessionStateManager.CompleteSessionAsync(sessionId);

        session.State.Should().Be(SessionState.Complete);
        _sessionRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Session>(s => s.State == SessionState.Complete)), Times.Once);
    }

    [Fact]
    public async Task CompleteSessionAsync_WithNonActiveSession_ThrowsException()
    {
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            State = SessionState.Draft
        };

        _sessionRepositoryMock.Setup(r => r.GetByIdAsync(sessionId)).ReturnsAsync(session);

        var act = async () => await _sessionStateManager.CompleteSessionAsync(sessionId);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
