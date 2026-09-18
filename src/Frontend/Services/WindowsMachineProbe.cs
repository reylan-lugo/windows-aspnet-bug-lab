using System.Management;

namespace Frontend.Services;

public sealed class WindowsMachineProbe
{
    public string GetOperatingSystemCaption()
    {
        using var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
        return searcher.Get()
            .Cast<ManagementObject>()
            .Select(result => result["Caption"]?.ToString())
            .FirstOrDefault(caption => !string.IsNullOrWhiteSpace(caption))
            ?? "Unknown Windows version";
    }
}
