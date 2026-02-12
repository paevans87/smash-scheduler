using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Web;

public class IndexedDbMatchRepository(SmashSchedulerDb database) : IMatchRepository
{
    public async Task<Match?> GetByIdAsync(Guid id)
    {
        await database.OpenAsync();
        return await database.Matches.GetAsync<string, Match>(id.ToString());
    }

    public async Task<List<Match>> GetBySessionIdAsync(Guid sessionId)
    {
        await database.OpenAsync();
        var allMatches = await database.Matches.GetAllAsync<Match>();
        return allMatches?.Where(m => m.SessionId == sessionId).ToList() ?? new List<Match>();
    }

    public async Task InsertAsync(Match match)
    {
        await database.OpenAsync();
        await database.Matches.AddAsync(match);
    }

    public async Task UpdateAsync(Match match)
    {
        await database.OpenAsync();
        await database.Matches.PutAsync(match);
    }

    public async Task DeleteAsync(Guid id)
    {
        await database.OpenAsync();
        await database.Matches.DeleteAsync<string>(id.ToString());
    }
}
