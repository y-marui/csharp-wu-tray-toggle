using System.Text.Json;
using System.Text.Json.Serialization;

namespace WuTrayToggle;

/// <summary>
/// ユーザーごとの設定。<c>%APPDATA%\WuTrayToggle\settings.json</c> に保存する。
/// 旧版の <c>language.txt</c>（言語コードだけを書いたテキスト）が残っていれば、初回の読み込みで JSON に移行して削除する。
/// </summary>
internal static class AppSettings
{
    private static readonly string DefaultDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "WuTrayToggle");

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string? GetLanguageOverride()
    {
        return GetLanguageOverride(DefaultDirectory);
    }

    public static bool SetLanguageOverride(string? code)
    {
        return SetLanguageOverride(code, DefaultDirectory);
    }

    // 保存先を差し替えられるようにするための内部オーバーロード（テスト用。挙動は同じ）
    internal static string? GetLanguageOverride(string directory)
    {
        return Load(directory).Language;
    }

    internal static bool SetLanguageOverride(string? code, string directory)
    {
        var data = Load(directory);
        data.Language = code;
        return Save(data, directory);
    }

    private static SettingsData Load(string directory)
    {
        try
        {
            var filePath = Path.Combine(directory, "settings.json");
            if (File.Exists(filePath))
            {
                return JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(filePath), JsonOptions) ?? new SettingsData();
            }

            var legacyPath = Path.Combine(directory, "language.txt");
            if (File.Exists(legacyPath))
            {
                return MigrateLegacyLanguageFile(legacyPath, directory);
            }
        }
        catch (Exception ex) when (IsReadException(ex))
        {
            // 設定の不備でトレイアプリを止めない（既定値で動く）
        }

        return new SettingsData();
    }

    private static SettingsData MigrateLegacyLanguageFile(string legacyPath, string directory)
    {
        var code = File.ReadAllText(legacyPath).Trim();
        var data = new SettingsData { Language = code.Length == 0 ? null : code };

        // JSON に保存できた場合だけ旧ファイルを消す（保存できなければ、次回また移行を試みる）
        if (Save(data, directory))
        {
            File.Delete(legacyPath);
        }

        return data;
    }

    private static bool Save(SettingsData data, string directory)
    {
        try
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "settings.json"), JsonSerializer.Serialize(data, JsonOptions) + "\n");
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
