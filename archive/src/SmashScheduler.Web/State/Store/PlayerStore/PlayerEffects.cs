using Fluxor;
using SmashScheduler.Application.Services.PlayerManagement;

namespace SmashScheduler.Web.State.Store.PlayerStore;

public class PlayerEffects(IPlayerService playerService)
{
    [EffectMethod]
    public async Task HandleLoadPlayersAction(LoadPlayersAction action, IDispatcher dispatcher)
    {
        try
        {
            var players = await playerService.GetByClubIdAsync(action.ClubId);
            dispatcher.Dispatch(new LoadPlayersSuccessAction(players));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadPlayersFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleCreatePlayerAction(CreatePlayerAction action, IDispatcher dispatcher)
    {
        try
        {
            var player = await playerService.CreatePlayerAsync(
                action.ClubId,
                action.Name,
                action.SkillLevel,
                action.Gender,
                action.PlayStyle);
            dispatcher.Dispatch(new CreatePlayerSuccessAction(player));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadPlayersFailureAction(ex.Message));
        }
    }
}
