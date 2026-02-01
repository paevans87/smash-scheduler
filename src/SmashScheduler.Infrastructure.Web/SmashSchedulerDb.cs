using CloudNimble.BlazorEssentials.IndexedDb;
using Microsoft.JSInterop;

namespace SmashScheduler.Infrastructure.Web;

public class SmashSchedulerDb : IndexedDbDatabase
{
    [ObjectStore(AutoIncrementKeys = false)]
    public IndexedDbObjectStore Clubs { get; set; } = null!;

    [ObjectStore(AutoIncrementKeys = false)]
    [Index(Name = "clubId", Path = "clubId")]
    public IndexedDbObjectStore Players { get; set; } = null!;

    [ObjectStore(AutoIncrementKeys = false)]
    [Index(Name = "clubId", Path = "clubId")]
    public IndexedDbObjectStore Sessions { get; set; } = null!;

    [ObjectStore(AutoIncrementKeys = false)]
    [Index(Name = "sessionId", Path = "sessionId")]
    public IndexedDbObjectStore Matches { get; set; } = null!;

    public SmashSchedulerDb(IJSRuntime jsRuntime) : base(jsRuntime)
    {
        Name = "SmashSchedulerDb";
        Version = 1;
    }
}
