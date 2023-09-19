namespace GameRank.Crawler.Configs;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

public sealed class CrawlerConfig
{
    public string StoragePath { get; init; } = string.Empty;
    
    public static bool TryLoad([MaybeNullWhen(false)] out CrawlerConfig config)
    {
        config = null;

        string fileName = "config.json";
        if (File.Exists(fileName) == false)
        {
            return false;
        }

        var json = File.ReadAllText(fileName);
        config = JsonSerializer.Deserialize<CrawlerConfig>(json);
        return config != null;
    }
}
