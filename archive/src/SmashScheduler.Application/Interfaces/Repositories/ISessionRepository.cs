namespace SmashScheduler.Application.Interfaces.Repositories;

using SmashScheduler.Domain.Entities;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id);
    Task<List<Session>> GetAllAsync();
    Task<List<Session>> GetByClubIdAsync(Guid clubId);
    Task InsertAsync(Session session);
    Task UpdateAsync(Session session);
    Task DeleteAsync(Guid id);
}
