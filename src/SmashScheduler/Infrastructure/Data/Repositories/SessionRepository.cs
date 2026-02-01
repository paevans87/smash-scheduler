using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly SmashSchedulerDbContext _context;

    public SessionRepository(SmashSchedulerDbContext context)
    {
        _context = context;
    }

    public async Task<Session?> GetByIdAsync(Guid id)
    {
        return await _context.Database.Table<Session>()
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Session>> GetByClubIdAsync(Guid clubId)
    {
        return await _context.Database.Table<Session>()
            .Where(s => s.ClubId == clubId)
            .OrderByDescending(s => s.ScheduledDateTime)
            .ToListAsync();
    }

    public async Task<Session?> GetActiveSessionAsync(Guid clubId)
    {
        return await _context.Database.Table<Session>()
            .Where(s => s.ClubId == clubId && s.State == SessionState.Active)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Session>> GetAllAsync()
    {
        return await _context.Database.Table<Session>().ToListAsync();
    }

    public async Task<int> InsertAsync(Session session)
    {
        session.CreatedAt = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;
        return await _context.Database.InsertAsync(session);
    }

    public async Task<int> UpdateAsync(Session session)
    {
        session.UpdatedAt = DateTime.UtcNow;
        return await _context.Database.UpdateAsync(session);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        return await _context.Database.Table<Session>()
            .Where(s => s.Id == id)
            .DeleteAsync();
    }

    public async Task<List<SessionPlayer>> GetSessionPlayersAsync(Guid sessionId)
    {
        return await _context.Database.Table<SessionPlayer>()
            .Where(sp => sp.SessionId == sessionId)
            .ToListAsync();
    }

    public async Task<int> AddPlayerToSessionAsync(SessionPlayer sessionPlayer)
    {
        sessionPlayer.JoinedAt = DateTime.UtcNow;
        return await _context.Database.InsertAsync(sessionPlayer);
    }

    public async Task<int> RemovePlayerFromSessionAsync(Guid sessionId, Guid playerId)
    {
        return await _context.Database.Table<SessionPlayer>()
            .Where(sp => sp.SessionId == sessionId && sp.PlayerId == playerId)
            .DeleteAsync();
    }

    public async Task<int> UpdateSessionPlayerAsync(SessionPlayer sessionPlayer)
    {
        return await _context.Database.UpdateAsync(sessionPlayer);
    }
}
