using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Web;

public class IndexedDbSessionRepository(SmashSchedulerDb database) : ISessionRepository
{
    public async Task<Session?> GetByIdAsync(Guid id)
    {
        await database.OpenAsync();
        return await database.Sessions.GetAsync<string, Session>(id.ToString());
    }

    public async Task<List<Session>> GetAllAsync()
    {
        await database.OpenAsync();
        var sessions = await database.Sessions.GetAllAsync<Session>();
        return sessions?.ToList() ?? new List<Session>();
    }

    public async Task<List<Session>> GetByClubIdAsync(Guid clubId)
    {
        await database.OpenAsync();
        var allSessions = await database.Sessions.GetAllAsync<Session>();
        return allSessions?.Where(s => s.ClubId == clubId).ToList() ?? new List<Session>();
    }

    public async Task InsertAsync(Session session)
    {
        session.CreatedAt = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;
        await database.OpenAsync();
        await database.Sessions.AddAsync(session);
    }

    public async Task UpdateAsync(Session session)
    {
        session.UpdatedAt = DateTime.UtcNow;
        await database.OpenAsync();
        await database.Sessions.PutAsync(session);
    }

    public async Task DeleteAsync(Guid id)
    {
        await database.OpenAsync();
        await database.Sessions.DeleteAsync<string>(id.ToString());
    }
}
