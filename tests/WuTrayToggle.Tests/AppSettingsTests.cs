namespace WuTrayToggle.Tests;

public sealed class AppSettingsTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "WuTrayToggle.Tests", Guid.NewGuid().ToString("N"));

    private string SettingsFile => Path.Combine(_directory, "settings.json");

    private string LegacyFile => Path.Combine(_directory, "language.txt");

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }

    [Fact]
    public void Get_ReturnsNull_WhenNothingIsStored()
    {
        Assert.Null(AppSettings.GetLanguageOverride(_directory));
    }

    [Fact]
    public void SetThenGet_RoundTrips()
    {
        Assert.True(AppSettings.SetLanguageOverride("fr", _directory));

        Assert.Equal("fr", AppSettings.GetLanguageOverride(_directory));
    }

    [Fact]
    public void Set_Null_RestoresSystemDefault()
    {
        AppSettings.SetLanguageOverride("ja", _directory);

        Assert.True(AppSettings.SetLanguageOverride(null, _directory));

        Assert.Null(AppSettings.GetLanguageOverride(_directory));
    }

    [Fact]
    public void LegacyFile_IsMigratedToJson_ThenDeleted()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(LegacyFile, "es\n");

        Assert.Equal("es", AppSettings.GetLanguageOverride(_directory));

        Assert.True(File.Exists(SettingsFile));
        Assert.Contains("\"es\"", File.ReadAllText(SettingsFile), StringComparison.Ordinal);
        Assert.False(File.Exists(LegacyFile));
    }

    [Fact]
    public void LegacyFile_IsKept_WhenJsonCannotBeWritten()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(LegacyFile, "pt");
        // settings.json の位置にディレクトリを置いて、保存を失敗させる（File.Exists は false なので移行に進む）
        Directory.CreateDirectory(SettingsFile);

        Assert.Equal("pt", AppSettings.GetLanguageOverride(_directory));

        Assert.True(File.Exists(LegacyFile));
    }

    [Fact]
    public void EmptyLegacyFile_MeansSystemDefault()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(LegacyFile, "  \n");

        Assert.Null(AppSettings.GetLanguageOverride(_directory));
        Assert.False(File.Exists(LegacyFile));
    }

    [Fact]
    public void CorruptJson_FallsBackToDefaults()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(SettingsFile, "{ not json");

        Assert.Null(AppSettings.GetLanguageOverride(_directory));
    }

    [Fact]
    public void CorruptJson_IsOverwrittenBySave()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(SettingsFile, "{ not json");

        Assert.True(AppSettings.SetLanguageOverride("en", _directory));

        Assert.Equal("en", AppSettings.GetLanguageOverride(_directory));
    }

    [Fact]
    public void Set_ReturnsFalseWithoutThrowing_WhenFileIsReadOnly()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(SettingsFile, "{}");
        File.SetAttributes(SettingsFile, FileAttributes.ReadOnly);

        try
        {
            Assert.False(AppSettings.SetLanguageOverride("de", _directory));
        }
        finally
        {
            File.SetAttributes(SettingsFile, FileAttributes.Normal);
        }
    }
}
