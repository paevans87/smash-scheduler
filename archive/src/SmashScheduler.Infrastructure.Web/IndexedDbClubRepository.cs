using SmashScheduler.Application.Interfaces.Repositories;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Web;

public class IndexedDbClubRepository(SmashSchedulerDb database) : IClubRepository
{
    public async Task<Club?> GetByIdAsync(Guid id)
    {
        await database.OpenAsync();
        return await database.Clubs.GetAsync<string, Club>(id.ToString());
    }

    public async Task<List<Club>> GetAllAsync()
    {
        await database.OpenAsync();
        var clubs = await database.Clubs.GetAllAsync<Club>();
        return clubs?.ToList() ?? new List<Club>();
    }

    public async Task InsertAsync(Club club)
    {
        club.CreatedAt = DateTime.UtcNow;
        club.UpdatedAt = DateTime.UtcNow;
        await database.OpenAsync();
        await database.Clubs.AddAsync(club);
    }

    public async Task UpdateAsync(Club club)
    {
        club.UpdatedAt = DateTime.UtcNow;
        await database.OpenAsync();
        await database.Clubs.PutAsync(club);
    }

    public async Task DeleteAsync(Guid id)
    {
        await database.OpenAsync();
        await database.Clubs.DeleteAsync<string>(id.ToString());
    }
}
