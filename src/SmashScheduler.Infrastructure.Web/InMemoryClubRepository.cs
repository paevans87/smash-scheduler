using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Web;

public class InMemoryClubRepository : IClubRepository
{
    private readonly List<Club> _clubs = new();

    public Task<Club?> GetByIdAsync(Guid id) => Task.FromResult(_clubs.FirstOrDefault(c => c.Id == id));
    public Task<List<Club>> GetAllAsync() => Task.FromResult(_clubs.ToList());
    
    public Task InsertAsync(Club club)
    {
        club.CreatedAt = DateTime.UtcNow;
        club.UpdatedAt = DateTime.UtcNow;
        _clubs.Add(club);
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(Club club)
    {
        var existing = _clubs.FirstOrDefault(c => c.Id == club.Id);
        if (existing != null)
        {
            _clubs.Remove(existing);
            club.UpdatedAt = DateTime.UtcNow;
            _clubs.Add(club);
        }
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(Guid id)
    {
        var club = _clubs.FirstOrDefault(c => c.Id == id);
        if (club != null) _clubs.Remove(club);
        return Task.CompletedTask;
    }
}
