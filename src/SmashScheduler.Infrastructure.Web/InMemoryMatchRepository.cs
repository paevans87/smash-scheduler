using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Web;

public class InMemoryMatchRepository : IMatchRepository
{
    private readonly List<Match> _matches = new();

    public Task<Match?> GetByIdAsync(Guid id) => Task.FromResult(_matches.FirstOrDefault(m => m.Id == id));
    public Task<List<Match>> GetBySessionIdAsync(Guid sessionId) => Task.FromResult(_matches.Where(m => m.SessionId == sessionId).ToList());
    
    public Task InsertAsync(Match match)
    {
        _matches.Add(match);
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(Match match)
    {
        var existing = _matches.FirstOrDefault(m => m.Id == match.Id);
        if (existing != null)
        {
            _matches.Remove(existing);
            _matches.Add(match);
        }
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(Guid id)
    {
        var match = _matches.FirstOrDefault(m => m.Id == id);
        if (match != null) _matches.Remove(match);
        return Task.CompletedTask;
    }
}
