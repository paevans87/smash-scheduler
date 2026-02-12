namespace SmashScheduler.Application.Interfaces.Repositories;

using SmashScheduler.Domain.Entities;

public interface IClubRepository
{
    Task<Club?> GetByIdAsync(Guid id);
    Task<List<Club>> GetAllAsync();
    Task InsertAsync(Club club);
    Task UpdateAsync(Club club);
    Task DeleteAsync(Guid id);
}
