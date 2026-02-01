using Fluxor;
using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Web.State.Store.SessionStore;

[FeatureState]
public record SessionStoreState
{
    public IEnumerable<Session> Sessions { get; init; } = Enumerable.Empty<Session>();
    public Session? CurrentSession { get; init; }
    public IEnumerable<Match> ActiveMatches { get; init; } = Enumerable.Empty<Match>();
    public Dictionary<Guid, Player> PlayerLookup { get; init; } = new();
    public SessionState? FilterState { get; init; }
    public bool IsLoading { get; init; }
    public bool IsGeneratingMatches { get; init; }
    public string? ErrorMessage { get; init; }
}
