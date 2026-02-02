using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Web;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> _players = new();
    private readonly List<PlayerBlacklist> _blacklists = new();

    public Task<Player?> GetByIdAsync(Guid id) => Task.FromResult(_players.FirstOrDefault(p => p.Id == id));
    public Task<List<Player>> GetAllAsync() => Task.FromResult(_players.ToList());
    public Task<List<Player>> GetByClubIdAsync(Guid clubId) => Task.FromResult(_players.Where(p => p.ClubId == clubId).ToList());

    public Task InsertAsync(Player player)
    {
        player.CreatedAt = DateTime.UtcNow;
        player.UpdatedAt = DateTime.UtcNow;
        _players.Add(player);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Player player)
    {
        var existing = _players.FirstOrDefault(p => p.Id == player.Id);
        if (existing != null)
        {
            _players.Remove(existing);
            player.UpdatedAt = DateTime.UtcNow;
            _players.Add(player);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var player = _players.FirstOrDefault(p => p.Id == id);
        if (player != null) _players.Remove(player);
        return Task.CompletedTask;
    }

    public Task<List<PlayerBlacklist>> GetBlacklistsByPlayerIdAsync(Guid playerId)
    {
        return Task.FromResult(_blacklists.Where(b => b.PlayerId == playerId).ToList());
    }

    public Task AddToBlacklistAsync(PlayerBlacklist blacklist)
    {
        blacklist.CreatedAt = DateTime.UtcNow;
        _blacklists.Add(blacklist);
        return Task.CompletedTask;
    }

    public Task RemoveFromBlacklistAsync(Guid playerId, Guid blacklistedPlayerId)
    {
        var toRemove = _blacklists.FirstOrDefault(b => b.PlayerId == playerId && b.BlacklistedPlayerId == blacklistedPlayerId);
        if (toRemove != null) _blacklists.Remove(toRemove);
        return Task.CompletedTask;
    }
}
