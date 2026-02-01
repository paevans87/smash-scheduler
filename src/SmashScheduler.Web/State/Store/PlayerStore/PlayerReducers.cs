using Fluxor;

namespace SmashScheduler.Web.State.Store.PlayerStore;

public static class PlayerReducers
{
    [ReducerMethod]
    public static PlayerState ReduceLoadPlayersAction(PlayerState state, LoadPlayersAction action)
        => state with { IsLoading = true, ErrorMessage = null };

    [ReducerMethod]
    public static PlayerState ReduceLoadPlayersSuccessAction(PlayerState state, LoadPlayersSuccessAction action)
        => state with { Players = action.Players, IsLoading = false };

    [ReducerMethod]
    public static PlayerState ReduceLoadPlayersFailureAction(PlayerState state, LoadPlayersFailureAction action)
        => state with { IsLoading = false, ErrorMessage = action.Error };

    [ReducerMethod]
    public static PlayerState ReduceSelectPlayerAction(PlayerState state, SelectPlayerAction action)
        => state with { SelectedPlayer = state.Players.FirstOrDefault(p => p.Id == action.PlayerId) };

    [ReducerMethod]
    public static PlayerState ReduceCreatePlayerSuccessAction(PlayerState state, CreatePlayerSuccessAction action)
        => state with { Players = state.Players.Append(action.Player) };
}
