using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly SmashSchedulerDbContext _context;

    public ClubRepository(SmashSchedulerDbContext context)
    {
        _context = context;
    }

    public async Task<Club?> GetByIdAsync(Guid id)
    {
        return await _context.Database.Table<Club>()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Club>> GetAllAsync()
    {
        return await _context.Database.Table<Club>().ToListAsync();
    }

    public async Task<int> InsertAsync(Club club)
    {
        club.CreatedAt = DateTime.UtcNow;
        club.UpdatedAt = DateTime.UtcNow;
        return await _context.Database.InsertAsync(club);
    }

    public async Task<int> UpdateAsync(Club club)
    {
        club.UpdatedAt = DateTime.UtcNow;
        return await _context.Database.UpdateAsync(club);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        return await _context.Database.Table<Club>()
            .Where(c => c.Id == id)
            .DeleteAsync();
    }
}
