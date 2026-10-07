using System.ComponentModel;
using System.Diagnostics;

namespace WuTrayToggle;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly NotifyIcon _notifyIcon;
    private readonly ToolStripItem _checkItem;
    private readonly ToolStripItem _stopItem;
    private readonly ToolStripItem _startItem;
    private readonly ToolStripItem _settingsItem;
    private readonly ToolStripItem _exitItem;
    private bool _settingsOpen;

    public TrayApplicationContext()
    {
        var menu = new ContextMenuStrip();
        _checkItem = menu.Items.Add(string.Empty);
        menu.Items.Add(new ToolStripSeparator());
        _stopItem = menu.Items.Add(string.Empty);
        _startItem = menu.Items.Add(string.Empty);
        menu.Items.Add(new ToolStripSeparator());
        _settingsItem = menu.Items.Add(string.Empty);
        _exitItem = menu.Items.Add(string.Empty);

        _checkItem.Click += (_, _) => ShowStatus();
        _stopItem.Click += (_, _) => RunElevatedAction("--elevated-stop", TrayState.Stopped);
        _startItem.Click += (_, _) => RunElevatedAction("--elevated-start", TrayState.Running);
        _settingsItem.Click += (_, _) => ShowSettings();
        _exitItem.Click += (_, _) => ExitThread();

        menu.Opening += (_, _) => RefreshMenuText();

        _notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = menu,
            Visible = true,
        };

        RefreshMenuText();
        RefreshStatus();
    }

    private void RefreshMenuText()
    {
        _checkItem.Text = Strings.MenuCheckStatus;
        _stopItem.Text = Strings.MenuStop;
        _startItem.Text = Strings.MenuStart;
        _settingsItem.Text = Strings.MenuSettings;
        _exitItem.Text = Strings.MenuExit;
    }

    private void ShowSettings()
    {
        // 設定ウィンドウはモーダルで開くため、開いている間に「設定…」が再度押されても、2 つ目は開かない
        if (_settingsOpen)
        {
            return;
        }

        _settingsOpen = true;
        try
        {
            using var form = new SettingsForm();
            form.ShowDialog();
        }
        finally
        {
            _settingsOpen = false;
        }

        // 言語が変わった場合に、メニューとツールチップへすぐに反映する
        RefreshMenuText();
        RefreshStatus();
    }

    private static void ShowStatus()
    {
        MessageBox.Show(WindowsUpdateController.GetStatusReport(), Strings.StatusTitle);
    }

    private void RunElevatedAction(string argument, TrayState expectedState)
    {
        var exePath = Environment.ProcessPath;
        if (exePath is null)
        {
            return;
        }

        var cancelled = false;
        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = argument,
                UseShellExecute = true,
                Verb = "runas",
            });
            process?.WaitForExit();
        }
        catch (Win32Exception)
        {
            // UACでキャンセルされた場合 (ERROR_CANCELLED)
            cancelled = true;
        }

        RefreshStatus();

        if (cancelled)
        {
            ShowBalloon(Strings.BalloonCancelled, ToolTipIcon.Warning);
        }
        else if (WindowsUpdateController.GetState() == expectedState)
        {
            var message = expectedState == TrayState.Stopped
                ? Strings.BalloonStopped
                : Strings.BalloonResumed;
            ShowBalloon(message, ToolTipIcon.Info);
        }
        else
        {
            ShowBalloon(Strings.BalloonFailed, ToolTipIcon.Error);
        }
    }

    private void ShowBalloon(string text, ToolTipIcon icon)
    {
        _notifyIcon.BalloonTipTitle = Strings.TrayTitle;
        _notifyIcon.BalloonTipText = text;
        _notifyIcon.BalloonTipIcon = icon;
        _notifyIcon.ShowBalloonTip(3000);
    }

    private void RefreshStatus()
    {
        var oldIcon = _notifyIcon.Icon;
        _notifyIcon.Icon = IconFactory.Create(WindowsUpdateController.GetState());
        _notifyIcon.Text = WindowsUpdateController.GetTrayText();
        oldIcon?.Dispose();
    }

    protected override void ExitThreadCore()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        base.ExitThreadCore();
    }
}
