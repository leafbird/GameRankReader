namespace GameRank.Core;

public sealed record TotalRankData
{
    public DateTime Date { get; init; }
    public List<SingleRankData> Ranks { get; } = new(); 
}