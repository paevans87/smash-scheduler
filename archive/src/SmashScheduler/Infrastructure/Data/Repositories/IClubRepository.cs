using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Infrastructure.Data.Repositories;

public interface IClubRepository
{
    Task<Club?> GetByIdAsync(Guid id);
    Task<List<Club>> GetAllAsync();
    Task<int> InsertAsync(Club club);
    Task<int> UpdateAsync(Club club);
    Task<int> DeleteAsync(Guid id);
}
