using Fluxor;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Web.State.Store.PlayerStore;

[FeatureState]
public record PlayerState
{
    public IEnumerable<Player> Players { get; init; } = Enumerable.Empty<Player>();
    public Player? SelectedPlayer { get; init; }
    public bool IsLoading { get; init; }
    public string? ErrorMessage { get; init; }
}
