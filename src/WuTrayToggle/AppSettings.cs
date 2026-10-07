using System.Text.Json;
using System.Text.Json.Serialization;

namespace WuTrayToggle;

/// <summary>
/// ユーザーごとの設定。<c>%APPDATA%\WuTrayToggle\settings.json</c> に保存する。
/// 旧版の <c>language.txt</c>（言語コードだけを書いたテキスト）が残っていれば、初回の読み込みで JSON に移行して削除する。
/// </summary>
internal static class AppSettings
{
    private static readonly string SettingsDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "WuTrayToggle");

    private static readonly string FilePath = Path.Combine(SettingsDirectory, "settings.json");
    private static readonly string LegacyLanguageFilePath = Path.Combine(SettingsDirectory, "language.txt");
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string? GetLanguageOverride()
    {
        return Load().Language;
    }

    public static bool SetLanguageOverride(string? code)
    {
        var data = Load();
        data.Language = code;
        return Save(data);
    }

    private static SettingsData Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                return JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(FilePath), JsonOptions) ?? new SettingsData();
            }

            if (File.Exists(LegacyLanguageFilePath))
            {
                return MigrateLegacyLanguageFile();
            }
        }
        catch (Exception ex) when (IsReadException(ex))
        {
            // 設定の不備でトレイアプリを止めない（既定値で動く）
        }

        return new SettingsData();
    }

    private static SettingsData MigrateLegacyLanguageFile()
    {
        var code = File.ReadAllText(LegacyLanguageFilePath).Trim();
        var data = new SettingsData { Language = code.Length == 0 ? null : code };

        // JSON に保存できた場合だけ旧ファイルを消す（保存できなければ、次回また移行を試みる）
        if (Save(data))
        {
            File.Delete(LegacyLanguageFilePath);
        }

        return data;
    }

    private static bool Save(SettingsData data)
    {
        try
        {
            Directory.CreateDirectory(SettingsDirectory);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(data, JsonOptions) + "\n");
            return true;
        }
        catch (Exception ex) when (IsFileSystemException(ex))
        {
            return false;
        }
    }

    private static bool IsFileSystemException(Exception exception)
    {
        return exception is IOException
            or UnauthorizedAccessException
            or System.Security.SecurityException;
    }

    private static bool IsReadException(Exception exception)
    {
        return IsFileSystemException(exception) || exception is JsonException;
    }

    private sealed class SettingsData
    {
        [JsonPropertyName("language")]
        public string? Language { get; set; }
    }
}
