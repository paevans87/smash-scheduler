namespace SmashScheduler.Application.Interfaces.Repositories;

using SmashScheduler.Domain.Entities;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id);
    Task<List<Match>> GetBySessionIdAsync(Guid sessionId);
    Task InsertAsync(Match match);
    Task UpdateAsync(Match match);
    Task DeleteAsync(Guid id);
}
