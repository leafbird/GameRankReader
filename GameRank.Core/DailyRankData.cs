namespace GameRank.Core;

using System.Text.Json;
using System.Text.Json.Serialization;
using GameRank.Core.Configs;

public sealed record DailyRankData 
{
    public DateTime Date { get; init; }
    public required Uri Source { get; init; }
    [JsonIgnore]
    public string SourceHost => this.Source.Host;
    public List<SingleRankData> Ranks { get; } = new();
    
    public static DailyRankData? FromString(string json)
    {
        return JsonSerializer.Deserialize<DailyRankData>(json, JsonOption.Default);
    }

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(this, JsonOption.Default);
    }
}