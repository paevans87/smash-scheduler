namespace SmashScheduler.Application.Services.Matchmaking.Models;

public record MatchPreviewResult(List<MatchCandidate> Matches, bool SaveAsDraft);
