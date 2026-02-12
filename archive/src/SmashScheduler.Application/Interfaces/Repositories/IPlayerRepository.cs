namespace SmashScheduler.Application.Interfaces.Repositories;

using SmashScheduler.Domain.Entities;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id);
    Task<List<Player>> GetAllAsync();
    Task<List<Player>> GetByClubIdAsync(Guid clubId);
    Task InsertAsync(Player player);
    Task UpdateAsync(Player player);
    Task DeleteAsync(Guid id);
    Task<List<PlayerBlacklist>> GetBlacklistsByPlayerIdAsync(Guid playerId);
    Task AddToBlacklistAsync(PlayerBlacklist blacklist);
    Task RemoveFromBlacklistAsync(Guid playerId, Guid blacklistedPlayerId);
}
