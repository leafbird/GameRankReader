namespace GameRank.Core;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public sealed record DailyRankData 
{
    private static readonly JsonSerializerOptions JsonOption;

    static DailyRankData()
    {
        JsonOption = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // 모든 코드에 escape 처리를 제거.
            WriteIndented = true, // 파일에 저장할 때, 들여쓰기를 해서 저장하도록 설정
        };
    }

    public DateTime Date { get; init; }
    public List<SingleRankData> Ranks { get; } = new();
    
    public static DailyRankData? FromString(string json)
    {
        return JsonSerializer.Deserialize<DailyRankData>(json, JsonOption);
    }

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(this, JsonOption);
    }
}