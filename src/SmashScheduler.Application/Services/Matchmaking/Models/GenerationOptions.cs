using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Application.Services.Matchmaking.Models;

public record GenerationOptions(
    List<int>? ExcludeCourtNumbers = null,
    GenerationStrategy Strategy = GenerationStrategy.ClubDefault,
    GenderFilter GenderFilter = GenderFilter.Any);
