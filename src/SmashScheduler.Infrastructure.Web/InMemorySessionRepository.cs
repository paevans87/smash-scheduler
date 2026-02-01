using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Web;

public class InMemorySessionRepository : ISessionRepository
{
    private readonly List<Session> _sessions = new();

    public Task<Session?> GetByIdAsync(Guid id) => Task.FromResult(_sessions.FirstOrDefault(s => s.Id == id));
    public Task<List<Session>> GetAllAsync() => Task.FromResult(_sessions.ToList());
    public Task<List<Session>> GetByClubIdAsync(Guid clubId) => Task.FromResult(_sessions.Where(s => s.ClubId == clubId).ToList());
    
    public Task InsertAsync(Session session)
    {
        session.CreatedAt = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;
        _sessions.Add(session);
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(Session session)
    {
        var existing = _sessions.FirstOrDefault(s => s.Id == session.Id);
        if (existing != null)
        {
            _sessions.Remove(existing);
            session.UpdatedAt = DateTime.UtcNow;
            _sessions.Add(session);
        }
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(Guid id)
    {
        var session = _sessions.FirstOrDefault(s => s.Id == id);
        if (session != null) _sessions.Remove(session);
        return Task.CompletedTask;
    }
}
