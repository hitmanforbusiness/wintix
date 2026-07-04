using Microsoft.Win32;
using System.Runtime.InteropServices;
using Wintix.Helpers;

namespace Wintix.Helpers;

public static class HardwareInfoHelper
{
    public static string GetCpuName()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            var name = key?.GetValue("ProcessorNameString") as string;
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name.Trim();
            }
        }
        catch
        {
            // ignored
        }

        return $"{Environment.ProcessorCount} cores · x64";
    }

    public static string GetGpuName()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000");
            var name = key?.GetValue("DriverDesc") as string;
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name.Trim();
            }
        }
        catch
        {
            // ignored
        }

        return "DirectX 12 · Hardware accelerated";
    }

    public static string GetMotherboardInfo()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            var manufacturer = key?.GetValue("BaseBoardManufacturer") as string;
            var product = key?.GetValue("BaseBoardProduct") as string;
            if (!string.IsNullOrWhiteSpace(manufacturer) || !string.IsNullOrWhiteSpace(product))
            {
                return $"{manufacturer} {product}".Trim();
            }
        }
        catch
        {
            // ignored
        }

        return Environment.MachineName;
    }

    public static (long Total, long Used, long Free, int UsagePercent) GetPrimaryDriveStats()
    {
        try
        {
            var drive = DriveInfo.GetDrives()
                .Where(d => d is { IsReady: true, DriveType: DriveType.Fixed })
                .OrderByDescending(d => d.TotalSize)
                .FirstOrDefault();

            if (drive is null)
            {
                return (0, 0, 0, 0);
            }

            var total = drive.TotalSize;
            var free = drive.AvailableFreeSpace;
            var used = total - free;
            var pct = total > 0 ? (int)Math.Round(used * 100d / total) : 0;
            return (total, used, free, pct);
        }
        catch
        {
            return (0, 0, 0, 0);
        }
    }

    public static IReadOnlyList<double> BuildSparkline(int seed, int points = 12)
    {
        var random = new Random(seed);
        var values = new double[points];
        var current = random.Next(20, 60);

        for (var i = 0; i < points; i++)
        {
            current = Math.Clamp(current + random.Next(-8, 9), 5, 95);
            values[i] = current;
        }

        return values;
    }

    public static string GetHealthStatus(int score)
    {
        return score switch
        {
            >= 90 => "Excellent",
            >= 75 => "Good",
            >= 60 => "Fair",
            _ => "Needs attention"
        };
    }

    public static string GetHealthStatusLocalized(int score, Func<string, string> t)
    {
        return score switch
        {
            >= 90 => t("Dashboard.Health.Excellent"),
            >= 75 => t("Dashboard.Health.Good"),
            >= 60 => t("Dashboard.Health.Fair"),
            _ => t("Dashboard.Health.Attention")
        };
    }
}
