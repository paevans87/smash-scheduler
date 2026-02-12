using Fluxor;
using SmashScheduler.Application.Services.ClubManagement;

namespace SmashScheduler.Web.State.Store.ClubStore;

public class ClubEffects(IClubService clubService)
{
    [EffectMethod]
    public async Task HandleLoadClubsAction(LoadClubsAction action, IDispatcher dispatcher)
    {
        try
        {
            var clubs = await clubService.GetAllClubsAsync();
            dispatcher.Dispatch(new LoadClubsSuccessAction(clubs));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadClubsFailureAction(ex.Message));
        }
    }

    [EffectMethod]
    public async Task HandleCreateClubAction(CreateClubAction action, IDispatcher dispatcher)
    {
        try
        {
            var club = await clubService.CreateClubAsync(action.Name, action.CourtCount, action.GameType, action.ScoringWeights, action.BlacklistMode);
            dispatcher.Dispatch(new CreateClubSuccessAction(club));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadClubsFailureAction(ex.Message));
        }
    }
}
