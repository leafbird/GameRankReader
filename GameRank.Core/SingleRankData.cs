namespace GameRank.Core;

public sealed record SingleRankData : IComparable<SingleRankData>
{
    public int Ranking { get; init; }
    public required string ImageUrl { get; init; }
    public required string Title { get; init; }
    public required string Publisher { get; init; }
    public required string SummaryPageUrl { get; init; }
    public float StarGrade { get; init; }
    public required string CategoryPageUrl { get; init; }
    public required string CategoryText { get; init; }

    public int CompareTo(SingleRankData? other)
    {
        return this.Ranking.CompareTo(other?.Ranking);
    }
}
