namespace GameRank.Crawler;

using Cs.Logging;
using Cs.Logging.Providers;
using GameRank.Core;
using GameRank.Core.Configs;
using GameRank.Crawler.Crawling;

internal class Program
{
    private static void Main(string[] args)
    {
        Log.Initialize(new SimpleFileLogProvider("log.txt"), LogLevelConfig.All);
        // 1. load config
        if (GameRankConfig.TryLoad(args, out var config) == false)
        {
            Log.Debug("Failed to load config.");
            return;
        }
        
        var client = new SeleniumClient();
        if (client.GetRankingData(out var rankData) == false)
        {
            Log.Debug("Failed to get ranking data.");
            return;
        }

        Log.Debug($"Date:{rankData.Date} #ranks:{rankData.Ranks.Count}");
        
        var storage = new FileStorage(config.StoragePath);
        storage.Save(rankData);
    }
}
