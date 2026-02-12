using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly SmashSchedulerDbContext _context;

    public MatchRepository(SmashSchedulerDbContext context)
    {
        _context = context;
    }

    public async Task<Match?> GetByIdAsync(Guid id)
    {
        return await _context.Database.Table<Match>()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Match>> GetBySessionIdAsync(Guid sessionId)
    {
        return await _context.Database.Table<Match>()
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.CourtNumber)
            .ToListAsync();
    }

    public async Task<List<Match>> GetAllAsync()
    {
        return await _context.Database.Table<Match>().ToListAsync();
    }

    public async Task<int> InsertAsync(Match match)
    {
        match.StartedAt = DateTime.UtcNow;
        return await _context.Database.InsertAsync(match);
    }

    public async Task<int> UpdateAsync(Match match)
    {
        return await _context.Database.UpdateAsync(match);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        return await _context.Database.Table<Match>()
            .Where(m => m.Id == id)
            .DeleteAsync();
    }

    public async Task<int> DeleteBySessionIdAsync(Guid sessionId)
    {
        return await _context.Database.Table<Match>()
            .Where(m => m.SessionId == sessionId)
            .DeleteAsync();
    }
}
