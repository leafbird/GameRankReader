namespace GameRank.Test.Tests;

using System;
using GameRank.Core;

[TestClass]
public class SingleRankDataTests
{
    [TestMethod]
    public void 랭킹순_정렬_테스트()
    {
        // Arrange
        var data1 = new SingleRankData
        {
            Ranking = 1,
            ImageUrl = "https://example.com/image1.jpg",
            Title = "Game 1",
            Publisher = "Publisher 1",
            SummaryPageUrl = "https://example.com/game1",
            StarGrade = 4.5f,
            CategoryPageUrl = "https://example.com/category1",
            CategoryText = "Category 1"
        };
        var data2 = new SingleRankData
        {
            Ranking = 2,
            ImageUrl = "https://example.com/image2.jpg",
            Title = "Game 2",
            Publisher = "Publisher 2",
            SummaryPageUrl = "https://example.com/game2",
            StarGrade = 3.5f,
            CategoryPageUrl = "https://example.com/category2",
            CategoryText = "Category 2"
        };

        // Act
        var result1 = data1.CompareTo(data2);
        var result2 = data2.CompareTo(data1);
        var result3 = data1.CompareTo(data1);

        // Assert
        Assert.AreEqual(-1, result1);
        Assert.AreEqual(1, result2);
        Assert.AreEqual(0, result3);
    }

    [TestMethod]
    public void 기본_생성_확인()
    {
        // Arrange
        var data = new SingleRankData
        {
            Ranking = 1,
            ImageUrl = "https://example.com/image1.jpg",
            Title = "Game 1",
            Publisher = "Publisher 1",
            SummaryPageUrl = "https://example.com/game1",
            StarGrade = 4.5f,
            CategoryPageUrl = "https://example.com/category1",
            CategoryText = "Category 1"
        };

        // Assert
        Assert.AreEqual(1, data.Ranking);
        Assert.AreEqual("https://example.com/image1.jpg", data.ImageUrl);
        Assert.AreEqual("Game 1", data.Title);
        Assert.AreEqual("Publisher 1", data.Publisher);
        Assert.AreEqual("https://example.com/game1", data.SummaryPageUrl);
        Assert.AreEqual(4.5f, data.StarGrade);
        Assert.AreEqual("https://example.com/category1", data.CategoryPageUrl);
        Assert.AreEqual("Category 1", data.CategoryText);
    }
}