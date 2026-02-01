using Fluxor;
using SmashScheduler.Domain.Entities;

namespace SmashScheduler.Web.State.Store.ClubStore;

[FeatureState]
public record ClubState
{
    public IEnumerable<Club> Clubs { get; init; } = Enumerable.Empty<Club>();
    public Club? SelectedClub { get; init; }
    public bool IsLoading { get; init; }
    public string? ErrorMessage { get; init; }
}
