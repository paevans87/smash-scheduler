using SmashScheduler.Domain.Enums;

namespace SmashScheduler.Application.Services.Matchmaking.Models;

public record GenerationOptions(
    List<int>? ExcludeCourtNumbers = null,
    GenerationStrategy Strategy = GenerationStrategy.Equal,
    GenderFilter GenderFilter = GenderFilter.Any);
