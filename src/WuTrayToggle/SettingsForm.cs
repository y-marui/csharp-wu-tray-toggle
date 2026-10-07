namespace WuTrayToggle;

/// <summary>
/// 独立した「設定」ウィンドウ。表示言語と「ログイン時に自動起動」を設定する。
/// 「保存」で反映して閉じる。「キャンセル」では何も変えない。
/// </summary>
internal sealed class SettingsForm : Form
{
    private static readonly (AppLanguage? Language, Func<string> Name)[] LanguageOptions =
    {
        (null, () => Strings.MenuLanguageSystem),
        (AppLanguage.Japanese, () => Strings.LanguageNameJapanese),
        (AppLanguage.English, () => Strings.LanguageNameEnglish),
        (AppLanguage.Chinese, () => Strings.LanguageNameChinese),
        (AppLanguage.Hindi, () => Strings.LanguageNameHindi),
        (AppLanguage.Spanish, () => Strings.LanguageNameSpanish),
        (AppLanguage.French, () => Strings.LanguageNameFrench),
        (AppLanguage.Portuguese, () => Strings.LanguageNamePortuguese),
    };

    private readonly ComboBox _languageBox = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
    private readonly CheckBox _startupBox = new() { AutoSize = true };

    internal SettingsForm()
    {
        Text = Strings.SettingsTitle;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = true;
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(12);

        foreach (var option in LanguageOptions)
        {
            _languageBox.Items.Add(option.Name());
        }

        var currentOverride = Localization.UserOverride;
        _languageBox.SelectedIndex = Math.Max(0, Array.FindIndex(LanguageOptions, o => o.Language == currentOverride));

        _startupBox.Text = Strings.MenuStartup;
        _startupBox.Checked = ShortcutManager.IsStartupEnabled();

        var save = new Button { Text = Strings.ButtonSave, AutoSize = true, DialogResult = DialogResult.OK };
        var cancel = new Button { Text = Strings.ButtonCancel, AutoSize = true, DialogResult = DialogResult.Cancel };
        save.Click += (_, _) => Save();

        var layout = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            Dock = DockStyle.Fill,
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.Controls.Add(new Label { Text = Strings.MenuLanguage, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 6, 12, 6) }, 0, 0);
        layout.Controls.Add(_languageBox, 1, 0);
        layout.Controls.Add(_startupBox, 0, 1);
        layout.SetColumnSpan(_startupBox, 2);

        var buttons = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 12, 0, 0),
        };
        buttons.Controls.Add(cancel);
        buttons.Controls.Add(save);
        layout.Controls.Add(buttons, 0, 2);
        layout.SetColumnSpan(buttons, 2);

        Controls.Add(layout);
        AcceptButton = save;
        CancelButton = cancel;
    }

    private void Save()
    {
        var language = LanguageOptions[_languageBox.SelectedIndex].Language;
        if (language != Localization.UserOverride && !Localization.SetOverride(language))
        {
            // 保存できなかったときは、ウィンドウを閉じずに残す（ボタンの DialogResult による自動クローズを取り消す）
            MessageBox.Show(Strings.BalloonLanguageSaveFailed, Strings.TrayTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            DialogResult = DialogResult.None;
            return;
        }

        if (_startupBox.Checked != ShortcutManager.IsStartupEnabled())
        {
            if (_startupBox.Checked)
            {
                ShortcutManager.EnableStartup();
            }
            else
            {
                ShortcutManager.DisableStartup();
            }
        }
    }
}
