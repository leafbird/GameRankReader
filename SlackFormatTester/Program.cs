using GameRank.Core;
using GameRank.Core.Configs;
using GameRank.Reader;

if (GameRankConfig.TryLoad(out var config) == false)
{
    Console.WriteLine("Failed to load config.");
    return;
}

var storage = new FileStorage(config.StoragePath);
var rankData = storage.LoadLatest();
if (rankData is null)
{
    Console.WriteLine($"Failed to load rank data from {config.StoragePath}");
    return;
}

var slack = new SlackController(rankData);
var message = slack.ToSlackJson();
Console.WriteLine(message);