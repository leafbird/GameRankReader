namespace GameRank.Core;

using System.Text;

public sealed class FileStorage
{
    private const string LatestFileName = "latest.json";
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

    public void Save(DailyRankData rankData)
    {
        var date = rankData.Date;

        // 파일은 연 / 월 별로 폴더를 만들어 저장한다.
        var targetPath = Path.Combine(this.basePath, date.Year.ToString(), date.Month.ToString());
        if (Directory.Exists(targetPath) == false)
        {
            Directory.CreateDirectory(targetPath);
        }
        
        var fileName = Path.Combine(targetPath, BuildFileName(date));
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        var jsonString = rankData.ToJsonString();
        File.WriteAllText(fileName, jsonString, Encoding.UTF8);
        
        // latest.json 처리
        fileName = Path.Combine(this.basePath, LatestFileName);
        var latest = Load(fileName);
        if (latest is not null)
        {
            if (latest.Date >= rankData.Date)
            {
                return; // 갱신이 필요없다면 바로 종료. 
            }
            
            // latest.json 파일이 있다면 삭제한다.
            File.Delete(Path.Combine(this.basePath, LatestFileName));
        }

        File.WriteAllText(fileName, jsonString, Encoding.UTF8);

        Console.WriteLine(jsonString);
    }
    
    public DailyRankData? Load(DateOnly date)
    {
        var targetPath = Path.Combine(this.basePath, date.Year.ToString(), date.Month.ToString());
        var fileName = Path.Combine(targetPath, BuildFileName(date.ToDateTime(TimeOnly.MinValue)));
        
        return Load(fileName);
    }
    
    public DailyRankData? LoadLatest()
    {
        var fileName = Path.Combine(this.basePath, LatestFileName);
        return Load(fileName);
    }

    //// -----------------------------------------------------------------------------------------
    
    private static string BuildFileName(DateTime date)
    {
        return $"rank_{date:yyyyMMdd}.json";
    }

    private static DailyRankData? Load(string fileName)
    {
        if (File.Exists(fileName) == false)
        {
            return null;
        }

        var json = File.ReadAllText(fileName, Encoding.UTF8);
        var rankData = DailyRankData.FromString(json);
        if (rankData == null)
        {
            return null;
        }

        return rankData;
    }
}
