using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id);
    Task<List<Player>> GetByClubIdAsync(Guid clubId);
    Task<List<Player>> GetAllAsync();
    Task<int> InsertAsync(Player player);
    Task<int> UpdateAsync(Player player);
    Task<int> DeleteAsync(Guid id);
    Task<List<PlayerBlacklist>> GetBlacklistsByPlayerIdAsync(Guid playerId);
    Task<int> AddToBlacklistAsync(PlayerBlacklist blacklist);
    Task<int> RemoveFromBlacklistAsync(Guid playerId, Guid blacklistedPlayerId);
}
