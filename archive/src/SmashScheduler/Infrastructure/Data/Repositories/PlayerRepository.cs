using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly SmashSchedulerDbContext _context;

    public PlayerRepository(SmashSchedulerDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        return await _context.Database.Table<Player>()
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Player>> GetByClubIdAsync(Guid clubId)
    {
        return await _context.Database.Table<Player>()
            .Where(p => p.ClubId == clubId)
            .ToListAsync();
    }

    public async Task<List<Player>> GetAllAsync()
    {
        return await _context.Database.Table<Player>().ToListAsync();
    }

    public async Task<int> InsertAsync(Player player)
    {
        player.CreatedAt = DateTime.UtcNow;
        player.UpdatedAt = DateTime.UtcNow;
        return await _context.Database.InsertAsync(player);
    }

    public async Task<int> UpdateAsync(Player player)
    {
        player.UpdatedAt = DateTime.UtcNow;
        return await _context.Database.UpdateAsync(player);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        return await _context.Database.Table<Player>()
            .Where(p => p.Id == id)
            .DeleteAsync();
    }

    public async Task<List<PlayerBlacklist>> GetBlacklistsByPlayerIdAsync(Guid playerId)
    {
        return await _context.Database.Table<PlayerBlacklist>()
            .Where(pb => pb.PlayerId == playerId)
            .ToListAsync();
    }

    public async Task<int> AddToBlacklistAsync(PlayerBlacklist blacklist)
    {
        blacklist.CreatedAt = DateTime.UtcNow;
        return await _context.Database.InsertAsync(blacklist);
    }

    public async Task<int> RemoveFromBlacklistAsync(Guid playerId, Guid blacklistedPlayerId)
    {
        return await _context.Database.Table<PlayerBlacklist>()
            .Where(pb => pb.PlayerId == playerId && pb.BlacklistedPlayerId == blacklistedPlayerId)
            .DeleteAsync();
    }
}
