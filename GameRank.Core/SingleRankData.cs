namespace GameRank.Core;

public sealed record SingleRankData : IComparable<SingleRankData>
{
    public int Ranking { get; init; }
    public string ImageUrl { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Publisher { get; init; } = null!;
    public string SummaryPageUrl { get; init; } = string.Empty;
    public float StarGrade { get; init; }
    public string CategoryPageUrl { get; init; } = string.Empty;
    public string CategoryText { get; init; } = string.Empty;

    int IComparable<SingleRankData>.CompareTo(SingleRankData? other)
    {
        return this.Ranking.CompareTo(other?.Ranking);
    }
}
