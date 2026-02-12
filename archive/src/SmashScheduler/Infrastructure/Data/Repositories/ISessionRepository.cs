using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id);
    Task<List<Session>> GetByClubIdAsync(Guid clubId);
    Task<Session?> GetActiveSessionAsync(Guid clubId);
    Task<List<Session>> GetAllAsync();
    Task<int> InsertAsync(Session session);
    Task<int> UpdateAsync(Session session);
    Task<int> DeleteAsync(Guid id);
    Task<List<SessionPlayer>> GetSessionPlayersAsync(Guid sessionId);
    Task<int> AddPlayerToSessionAsync(SessionPlayer sessionPlayer);
    Task<int> RemovePlayerFromSessionAsync(Guid sessionId, Guid playerId);
    Task<int> UpdateSessionPlayerAsync(SessionPlayer sessionPlayer);
}
