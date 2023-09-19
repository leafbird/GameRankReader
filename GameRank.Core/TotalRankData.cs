namespace GameRank.Core;

using System.Text.Json;

public sealed record TotalRankData 
{
    private static readonly JsonSerializerOptions JsonOption;

    static TotalRankData()
    {
        JsonOption = new JsonSerializerOptions
        {
            WriteIndented = true, // 파일에 저장할 때, 들여쓰기를 해서 저장하도록 설정
        };
    }

    public DateTime Date { get; init; }
    public List<SingleRankData> Ranks { get; } = new();
    
    public static TotalRankData? FromString(string json)
    {
        return JsonSerializer.Deserialize<TotalRankData>(json, JsonOption);
    }

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(this, JsonOption);
    }
}