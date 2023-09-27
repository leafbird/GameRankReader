namespace GameRank.Crawler;

using GameRank.Core;
using GameRank.Core.Configs;
using GameRank.Crawler.Crawling;

internal class Program
{
    private static void Main(string[] args)
    {
        // 1. load config
        if (GameRankConfig.TryLoad(out var config) == false)
        {
            Console.WriteLine("Failed to load config.");
            return;
        }
        
        var client = new SeleniumClient();
        if (client.GetRankingData(out var rankData) == false)
        {
            Console.WriteLine("Failed to get ranking data.");
            return;
        }

        Console.WriteLine($"Date:{rankData.Date} #ranks:{rankData.Ranks.Count}");
        
        var storage = new FileStorage(config.StoragePath);
        storage.Save(rankData);
    }
}
