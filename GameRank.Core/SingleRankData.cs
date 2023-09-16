namespace GameRank.Core;

public sealed record SingleRankData
{
    public int Ranking { get; init; }
    public string ImageUrl { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Publisher { get; init; } = null!;
}
