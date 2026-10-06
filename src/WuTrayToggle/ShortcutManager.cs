using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace WuTrayToggle;

internal static class ShortcutManager
{
    // ログイン時の自動起動（スタートアップフォルダ）のショートカット。トレイのメニューから実行時に作る。
    // デスクトップ・スタートメニューのショートカットは MSI が作る（配布の入口は MSI に一本化している）
    private const string StartupShortcutFileName = "WU_TrayIcon.lnk";

    public static bool IsStartupEnabled()
    {
        return File.Exists(GetStartupShortcutPath());
    }

    public static void EnableStartup()
    {
        var exePath = Environment.ProcessPath;
        if (exePath is null)
        {
            return;
        }

        CreateShortcut(GetStartupShortcutPath(), exePath);
    }

    public static void DisableStartup()
    {
        var path = GetStartupShortcutPath();
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static string GetStartupShortcutPath()
    {
        var startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        return Path.Combine(startup, StartupShortcutFileName);
    }

    private static void CreateShortcut(string shortcutPath, string targetPath)
    {
        var shellLink = (IShellLinkW)new ShellLink();
        shellLink.SetPath(targetPath);
        shellLink.SetWorkingDirectory(Path.GetDirectoryName(targetPath) ?? string.Empty);
        shellLink.SetDescription("Windows Update Tray Toggle");

        var persistFile = (IPersistFile)shellLink;
        persistFile.Save(shortcutPath, fRemember: false);
    }

    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    private class ShellLink
    {
    }

    [ComImport]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IShellLinkW
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, IntPtr pfd, int fFlags);

        void GetIDList(out IntPtr ppidl);

        void SetIDList(IntPtr pidl);

        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);

        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);

        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);

        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

        void GetHotkey(out short pwHotkey);

        void SetHotkey(short wHotkey);

        void GetShowCmd(out int piShowCmd);

        void SetShowCmd(int iShowCmd);

        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

        void Resolve(IntPtr hwnd, int fFlags);

        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }
}
