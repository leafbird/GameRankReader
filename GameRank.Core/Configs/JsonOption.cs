namespace GameRank.Core.Configs;

using System.Text.Encodings.Web;
using System.Text.Json;

public static class JsonOption
{
    public static readonly JsonSerializerOptions Default;

    static JsonOption()
    {
        Default = new JsonSerializerOptions
        {
            // Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // 모든 코드에 escape 처리를 제거.
            // https://stackoverflow.com/questions/58003293/dotnet-core-system-text-json-unescape-unicode-string
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true, // 파일에 저장할 때, 들여쓰기를 해서 저장하도록 설정
        };
    }
}
