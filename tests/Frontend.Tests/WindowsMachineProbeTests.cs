using Frontend.Services;

namespace Frontend.Tests;

public sealed class WindowsMachineProbeTests
{
    [Fact]
    public void Reads_windows_operating_system_caption()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var caption = new WindowsMachineProbe().GetOperatingSystemCaption();

        Assert.NotEqual("Unknown Windows version", caption);
    }
}
