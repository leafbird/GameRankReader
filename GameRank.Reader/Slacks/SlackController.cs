namespace GameRank.Reader;

using GameRank.Core;
using SlackNet.Blocks;
using SlackNet.WebApi;

public sealed class SlackController
{
    //// private DailyRankData? rankData;

    public static Message ToSlackMessage(DailyRankData rankData)
    {
        var message = new Message
        {
            Text = $"*{rankData.Date:yyyy-MM-dd}*",
            Blocks = new List<Block>
            {
                new SectionBlock
                {
                    Text = new Markdown("Ranking"),
                },
                new DividerBlock(),
            },
        };

        foreach (var game in rankData.Ranks)
        {
            message.Blocks.Add(new SectionBlock
            {
                Text = new Markdown($"{game.Ranking}. {game.Title}"),
            });
        }

        return message;
    }
    
    public static string ToSlackJson(DailyRankData rankData)
    {
        var message = ToSlackMessage(rankData);
        return message.ToString() ?? string.Empty;
        // return JsonSerializer.Serialize(message);
    }
}
