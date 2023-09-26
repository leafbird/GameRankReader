namespace GameRank.Test.Tests;

using System.Text;
using GameRank.Core;

[TestClass]
public class FileStorageTests
{
    private string testPath = string.Empty;

    [TestInitialize]
    public void Initialize()
    {
        this.testPath = Path.Combine(Path.GetTempPath(), "FileStorageTests");
        if (Directory.Exists(this.testPath))
        {
            Directory.Delete(this.testPath, true);
        }
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(this.testPath))
        {
            Directory.Delete(this.testPath, true);
        }
    }

    [TestMethod]
    public void 폴더_생성_테스트()
    {
        // Arrange
        var storage = new FileStorage(this.testPath);
        var rankData = new DailyRankData
        {
            Date = DateTime.Now, 
            Source = new Uri("https://example.com").ToString(),
        };

        // Act
        storage.Save(rankData);

        // Assert
        var expectedPath = Path.Combine(this.testPath, rankData.Date.Year.ToString(), rankData.Date.Month.ToString());
        Assert.IsTrue(Directory.Exists(expectedPath));
    }

    [TestMethod]
    public void 저장내용_JSON_확인()
    {
        // Arrange
        var storage = new FileStorage(this.testPath);
        var rankData = new DailyRankData
        {
            Date = DateTime.Now, 
            Source = new Uri("https://example.com").ToString(),
        };
        // var expectedPath = Path.Combine(this.testPath, rankData.Date.Year.ToString(), rankData.Date.Month.ToString(), "latest.json");
        var expectedPath = Path.Combine(this.testPath, "latest.json");

        // Act
        storage.Save(rankData);

        // Assert
        Assert.IsTrue(File.Exists(expectedPath));
        var actualData = File.ReadAllText(expectedPath, Encoding.UTF8);
        var expectedData = rankData.ToJsonString();
        Assert.AreEqual(expectedData, actualData);
    }
}