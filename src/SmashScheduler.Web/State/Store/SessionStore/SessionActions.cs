using SmashScheduler.Domain.Entities;
using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Web.State.Store.SessionStore;

public record LoadSessionsAction(Guid ClubId);

public record LoadSessionsSuccessAction(IEnumerable<Session> Sessions);

public record LoadSessionsFailureAction(string Error);

public record LoadSessionAction(Guid SessionId);

public record LoadSessionSuccessAction(Session Session, IEnumerable<Match> Matches, Dictionary<Guid, Player> PlayerLookup);

public record CreateSessionAction(Guid ClubId);

public record CreateSessionSuccessAction(Session Session);

public record AddPlayerToSessionAction(Guid SessionId, Guid PlayerId);

public record AddPlayerSuccessAction(Session Session);

public record RemovePlayerFromSessionAction(Guid SessionId, Guid PlayerId);

public record RemovePlayerSuccessAction(Session Session);

public record ActivateSessionAction(Guid SessionId);

public record ActivateSessionSuccessAction;

public record GenerateMatchesAction(Guid SessionId);

public record GenerateMatchesSuccessAction(IEnumerable<Match> Matches);

public record CompleteMatchAction(Guid MatchId);

public record CompleteMatchSuccessAction(Guid MatchId);

public record EndSessionAction(Guid SessionId);

public record EndSessionSuccessAction;

public record SetSessionFilterAction(SessionState? Filter);
