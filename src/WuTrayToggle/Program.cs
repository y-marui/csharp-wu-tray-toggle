namespace WuTrayToggle;

internal static class Program
{
    private const string MutexName = "Global\\WuTrayToggle.SingleInstance";

    [STAThread]
    private static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "--disable-startup":
                    // Invoked by the MSI uninstaller: the Startup-folder autostart
                    // shortcut is created at runtime via the tray menu, so MSI has
                    // no record of it and won't remove it on its own.
                    ShortcutManager.DisableStartup();
                    return;
                case "--elevated-stop":
                    WindowsUpdateController.Stop();
                    return;
                case "--elevated-start":
                    WindowsUpdateController.Start();
                    return;
            }
        }

        using var mutex = new Mutex(initiallyOwned: true, MutexName, out var createdNew);
        if (!createdNew)
        {
            MessageBox.Show(Strings.AlreadyRunning, Strings.TrayTitle);
            return;
        }

        // 高 DPI は csproj の ApplicationHighDpiMode（PerMonitorV2）。ビジュアルスタイルなども一括で設定する
        ApplicationConfiguration.Initialize();
        Application.Run(new TrayApplicationContext());
    }
}
