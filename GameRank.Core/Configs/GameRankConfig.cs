namespace GameRank.Core.Configs;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

public sealed class GameRankConfig
{
    public string StoragePath { get; init; } = string.Empty;
    
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
        return config != null;
    }
}
