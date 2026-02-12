using SmashScheduler.Domain.Entities;
using SQLite;

namespace SmashScheduler.Infrastructure.Data;

public class SmashSchedulerDbContext
{
    private readonly SQLiteAsyncConnection _database;

    public SmashSchedulerDbContext(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        InitialiseDatabaseAsync().Wait();
    }

    private async Task InitialiseDatabaseAsync()
    {
        await _database.CreateTableAsync<Club>();
        await _database.CreateTableAsync<Player>();
        await _database.CreateTableAsync<Session>();
        await _database.CreateTableAsync<SessionPlayer>();
        await _database.CreateTableAsync<Match>();
        await _database.CreateTableAsync<PlayerBlacklist>();
    }

    public SQLiteAsyncConnection Database => _database;
}
