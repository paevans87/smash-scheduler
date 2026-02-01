using Fluxor;

namespace SmashScheduler.Web.State.Store.ClubStore;

public static class ClubReducers
{
    [ReducerMethod]
    public static ClubState ReduceLoadClubsAction(ClubState state, LoadClubsAction action)
        => state with { IsLoading = true, ErrorMessage = null };

    [ReducerMethod]
    public static ClubState ReduceLoadClubsSuccessAction(ClubState state, LoadClubsSuccessAction action)
        => state with { Clubs = action.Clubs, IsLoading = false };

    [ReducerMethod]
    public static ClubState ReduceLoadClubsFailureAction(ClubState state, LoadClubsFailureAction action)
        => state with { IsLoading = false, ErrorMessage = action.Error };

    [ReducerMethod]
    public static ClubState ReduceSelectClubAction(ClubState state, SelectClubAction action)
        => state with { SelectedClub = state.Clubs.FirstOrDefault(c => c.Id == action.ClubId) };

    [ReducerMethod]
    public static ClubState ReduceCreateClubSuccessAction(ClubState state, CreateClubSuccessAction action)
        => state with { Clubs = state.Clubs.Append(action.Club) };
}
