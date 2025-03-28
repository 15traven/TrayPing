using Microsoft.Win32;

namespace TrayPing
{
    public class Autolaunch
    {
        private const string AppName = "TrayPing";
        private const string RunRegKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string TaskManagerRegKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run";

        public static void Register()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RunRegKey, true);
            key.SetValue(AppName, Application.ExecutablePath);
        }

        public static void Toggle(bool enable)
        {
            string exePath = Application.ExecutablePath;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(TaskManagerRegKey, true);
            key.SetValue(AppName, enable ? new byte[] { 2, 0, 0, 0 } : new byte[] { 3, 0, 0, 0 }, RegistryValueKind.Binary);
        }

        public static bool IsEnabled()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(TaskManagerRegKey, true);
            if (key?.GetValue(AppName) is byte[] value)
            {
                return value[0] == 2;
            }

            return false;
        }
    }
}
