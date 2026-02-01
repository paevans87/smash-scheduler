using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Web.State.Store.ClubStore;

public record LoadClubsAction;

public record LoadClubsSuccessAction(IEnumerable<Club> Clubs);

public record LoadClubsFailureAction(string Error);

public record SelectClubAction(Guid ClubId);

public record CreateClubAction(string Name, int CourtCount, GameType GameType);

public record CreateClubSuccessAction(Club Club);
