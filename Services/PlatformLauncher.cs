using System.Diagnostics;

namespace kcsj.Services;

public static class PlatformLauncher
{
    public static void OpenFolder(string folderPath)
    {
        ProcessStartInfo startInfo;

        if (OperatingSystem.IsWindows())
        {
            startInfo = new ProcessStartInfo
            {
                FileName = folderPath,
                UseShellExecute = true
            };
        }
        else
        {
            startInfo = new ProcessStartInfo
            {
                FileName = OperatingSystem.IsMacOS() ? "open" : "xdg-open",
                UseShellExecute = false
            };
            startInfo.ArgumentList.Add(folderPath);
        }

        Process.Start(startInfo);
    }
}
