namespace GameRank.Reader;

using System.Text.Json;
using GameRank.Core;
using GameRank.Core.Configs;
using SlackNet.Blocks;
using SlackNet.WebApi;

public readonly ref struct SlackController
{
    private readonly DailyRankData rankData;
    
    public SlackController(DailyRankData rankData)
    {
        this.rankData = rankData;
    }
    
    public Message ToSlackMessage()
    {
        var message = new Message
        {
            Text = $"*{this.rankData.Date:yyyy-MM-dd}*",
            Blocks = new List<Block>
            {
                new SectionBlock
                {
                    Text = new Markdown("Ranking"),
                },
                new DividerBlock(),
            },
        };

        foreach (var game in this.rankData.Ranks)
        {
            message.Blocks.Add(new SectionBlock
            {
                Text = new Markdown($"{game.Ranking}. {game.Title}"),
            });
            
            message.Blocks.Add(new DividerBlock());
        }

        return message;
    }
    
    public string ToSlackJson()
    {
        var message = this.ToSlackMessage();
        // return message.ToString() ?? string.Empty;
        return JsonSerializer.Serialize(message, JsonOption.Default);
    }
}
