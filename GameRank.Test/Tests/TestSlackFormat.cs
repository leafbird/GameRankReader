using System.Diagnostics;

namespace GameRank.Test.Tests;

[TestClass]
public sealed class TestSlackFormat
{
    public required TestContext TestContext { get; init; }

    [TestInitialize]
    public void Initialize() 
    {
    }

    [TestMethod]
    public void 슬랙_데이터_포맷_테스트()
    {
        Trace.WriteLine("trace 테스트");
        Console.WriteLine("console 테스트");
        this.TestContext.WriteLine("test context 테스트");
    }
}
