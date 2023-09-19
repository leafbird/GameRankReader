namespace GameRank.Core;

using System.Text.Json;

public sealed class FileStorage
{
    private readonly string basePath;

    public FileStorage(string path)
    {
        this.basePath = path;
        if (Directory.Exists(this.basePath) == false)
        {
            Console.WriteLine($"create storage root path:{this.basePath}");
            Directory.CreateDirectory(this.basePath);
        }
    }

    public void Save(TotalRankData rankData)
    {
        var date = rankData.Date;

        // 파일은 연 / 월 별로 폴더를 만들어 저장한다.
        var targetPath = Path.Combine(this.basePath, date.Year.ToString(), date.Month.ToString());
        if (Directory.Exists(targetPath) == false)
        {
            Directory.CreateDirectory(targetPath);
        }
        
        var fileName = Path.Combine(targetPath, $"rank_{date:yyyyMMdd}.json");
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        File.WriteAllText(fileName, rankData.ToJsonString());
    }
}
