namespace GameRank.Core.Configs;

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

public static class JsonOption
{
    public static readonly JsonSerializerOptions Default;

    static JsonOption()
    {
        Default = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // 모든 코드에 escape 처리를 제거.
            WriteIndented = true, // 파일에 저장할 때, 들여쓰기를 해서 저장하도록 설정
        };
    }
}
