using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id);
    Task<List<Match>> GetBySessionIdAsync(Guid sessionId);
    Task<List<Match>> GetAllAsync();
    Task<int> InsertAsync(Match match);
    Task<int> UpdateAsync(Match match);
    Task<int> DeleteAsync(Guid id);
    Task<int> DeleteBySessionIdAsync(Guid sessionId);
}
