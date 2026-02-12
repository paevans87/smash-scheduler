using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Web.State.Store.PlayerStore;

public record LoadPlayersAction(Guid ClubId);

public record LoadPlayersSuccessAction(IEnumerable<Player> Players);

public record LoadPlayersFailureAction(string Error);

public record SelectPlayerAction(Guid PlayerId);

public record CreatePlayerAction(Guid ClubId, string Name, int SkillLevel, Gender Gender, PlayStylePreference PlayStyle);

public record CreatePlayerSuccessAction(Player Player);
