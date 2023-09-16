namespace GameRank.Crawler;

using GameRank.Crawler.Crawling;

internal class Program
{
    private static void Main(string[] args)
    {
        var client = new SeleniumClient();
        if (client.GetRankingData(out var result) == false)
        {
            Console.WriteLine("Failed to get ranking data.");
            return;
        }

        Console.WriteLine($"Date:{result.Date} #ranks:{result.Ranks.Count}");
    }
}
