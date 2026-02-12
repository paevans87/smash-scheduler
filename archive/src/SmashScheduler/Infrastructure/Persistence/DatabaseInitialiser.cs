using SmashScheduler.Infrastructure.Data;

namespace SmashScheduler.Infrastructure.Persistence;

public class DatabaseInitialiser
{
    private readonly SmashSchedulerDbContext _context;

    public DatabaseInitialiser(SmashSchedulerDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        await Task.CompletedTask;
    }
}
