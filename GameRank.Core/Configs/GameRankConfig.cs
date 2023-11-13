namespace GameRank.Core.Configs;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

public sealed class GameRankConfig
{
    public static GameRankConfig Instance { get; private set; } = new();

    public string StoragePath { get; init; } = string.Empty;
    public bool CreateLatestFile { get; init; }
    
    public static bool TryLoad(string[] args, [MaybeNullWhen(false)] out GameRankConfig config)
    {
        config = null;

        string fileName = args.FirstOrDefault() ?? "config.json";
        if (File.Exists(fileName) == false)
        {
            return false;
        }

        var json = File.ReadAllText(fileName);
        config = JsonSerializer.Deserialize<GameRankConfig>(json);
        if (config is null)
        {
            return false;
        }

        Instance = config;
        return true;
    }
    
    public static void SetForTest()
    {
        Instance = new GameRankConfig
        {
            StoragePath = Path.Combine(Path.GetTempPath(), "GameRank"),
            CreateLatestFile = true,
        };
    }
}
