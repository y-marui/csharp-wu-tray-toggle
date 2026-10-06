using System.ServiceProcess;
using Microsoft.Win32;

namespace WuTrayToggle;

internal static class WindowsUpdateController
{
    private const string PolicyKeyPath = @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";
    private const string ServiceKeyPath = @"SYSTEM\CurrentControlSet\Services\wuauserv";
    private const string ServiceName = "wuauserv";

    public static TrayState GetState()
    {
        return GetNoAutoUpdate() == 1 ? TrayState.Stopped : TrayState.Running;
    }

    public static string GetTrayText()
    {
        return GetState() == TrayState.Stopped
            ? Strings.TrayTextStopped
            : Strings.TrayTextRunning;
    }

    // 書式文字列は言語設定で実行時に切り替わり、呼ばれるのも「状態確認」の操作時だけなので、CompositeFormat にキャッシュしない
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1863:Use 'CompositeFormat'",
        Justification = "The format string is localized at runtime and this runs only on a user action.")]
    public static string GetStatusReport()
    {
        var policyText = GetNoAutoUpdate() == 1 ? Strings.PolicyStopped : Strings.PolicyRunning;
        return string.Format(
            Strings.StatusReportFormat,
            policyText,
            GetServiceStatusText(),
            Application.ProductVersion);
    }

    public static void Stop()
    {
        using (var key = OpenOrCreatePolicyKey())
        {
            key.SetValue("NoAutoUpdate", 1, RegistryValueKind.DWord);
        }

        try
        {
            using var service = new ServiceController(ServiceName);
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
            }
        }
        catch (InvalidOperationException)
        {
        }
        catch (System.ServiceProcess.TimeoutException)
        {
        }
    }

    public static void Start()
    {
        using (var key = OpenOrCreatePolicyKey())
        {
            key.SetValue("NoAutoUpdate", 0, RegistryValueKind.DWord);
        }

        SetServiceStartupType(manual: true);

        try
        {
            using var service = new ServiceController(ServiceName);
            if (service.Status != ServiceControllerStatus.Running)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
            }
        }
        catch (InvalidOperationException)
        {
        }
        catch (System.ServiceProcess.TimeoutException)
        {
        }
    }

    private static RegistryKey OpenOrCreatePolicyKey()
    {
        return Registry.LocalMachine.CreateSubKey(PolicyKeyPath)
            ?? throw new InvalidOperationException($@"Unable to open HKLM\{PolicyKeyPath}");
    }

    private static int GetNoAutoUpdate()
    {
        using var key = Registry.LocalMachine.OpenSubKey(PolicyKeyPath);
        return key?.GetValue("NoAutoUpdate") is int value ? value : 0;
    }

    private static string GetServiceStatusText()
    {
        try
        {
            using var service = new ServiceController(ServiceName);
            return service.Status == ServiceControllerStatus.Running ? Strings.ServiceRunning : Strings.ServiceStopped;
        }
        catch (InvalidOperationException)
        {
            return Strings.ServiceUnknown;
        }
    }

    private static void SetServiceStartupType(bool manual)
    {
        using var key = Registry.LocalMachine.OpenSubKey(ServiceKeyPath, writable: true);
        // Start: 2 = Automatic, 3 = Manual, 4 = Disabled
        key?.SetValue("Start", manual ? 3 : 2, RegistryValueKind.DWord);
    }
}
