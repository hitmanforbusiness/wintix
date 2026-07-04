using System.Runtime.InteropServices;
using Wintix.Helpers;
using Wintix.Localization;
using Wintix.Models;

namespace Wintix.Services;

public sealed class SystemOverviewService : ISystemOverviewService
{
    public Task<SystemOverviewData> GetOverviewAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
        var osDescription = RuntimeInformation.OSDescription.Trim();
        var (ramLoad, ramTotal, ramUsed) = SystemMetricsHelper.GetMemoryStats();
        var (diskTotal, diskUsed, diskFree, diskUsage) = HardwareInfoHelper.GetPrimaryDriveStats();

        var cpuUsage = Random.Shared.Next(18, 42);
        var networkLoad = Random.Shared.Next(8, 28);
        var healthScore = ComputeHealthScore(cpuUsage, ramLoad, diskUsage);
        var cpuName = HardwareInfoHelper.GetCpuName();
        var gpuName = HardwareInfoHelper.GetGpuName();
        var boardName = HardwareInfoHelper.GetMotherboardInfo();

        var cpuMetric = BuildCpuMetric(cpuUsage);
        var ramMetric = BuildRamMetric(ramLoad, ramTotal, ramUsed);
        var diskMetric = BuildDiskMetric(diskUsage, diskUsed, diskTotal, diskFree);
        var networkMetric = BuildNetworkMetric(networkLoad);

        var data = new SystemOverviewData
        {
            OperatingSystem = osDescription,
            Uptime = FormatUptime(uptime),
            CpuSummary = $"{cpuUsage}%",
            RamSummary = FormatHelper.FormatBytes(ramUsed),
            DiskSummary = $"{diskUsage}%",
            NetworkStatus = L.T("Dashboard.NetworkStatus"),
            HealthScore = healthScore.ToString(),
            HealthStatus = HardwareInfoHelper.GetHealthStatusLocalized(healthScore, L.T),
            LastScanSummary = L.T("Dashboard.Optimized"),
            LastScanTime = L.T("Dashboard.LastScan.Never"),
            Snapshots = [],
            UsageMetrics = [cpuMetric, ramMetric, diskMetric, networkMetric],
            CpuMetric = cpuMetric,
            RamMetric = ramMetric,
            DiskMetric = diskMetric,
            NetworkMetric = networkMetric,
            Components =
            [
                new SystemComponent
                {
                    Category = L.T("Dashboard.Component.Cpu"),
                    Name = cpuName,
                    Detail = $"{Environment.ProcessorCount} {L.T("Dashboard.Component.Cores")} · {L.T("Dashboard.Component.PerformanceMode")}",
                    SpecLine = L.T("Dashboard.Component.CpuSpec"),
                    StatusText = L.T("Dashboard.Component.Normal"),
                    IconGlyph = "\uE950"
                },
                new SystemComponent
                {
                    Category = L.T("Dashboard.Component.Gpu"),
                    Name = gpuName,
                    Detail = L.T("Dashboard.Component.GpuDetail"),
                    SpecLine = "DirectX 12",
                    StatusText = L.T("Dashboard.Component.Active"),
                    IconGlyph = "\uE7F4"
                },
                new SystemComponent
                {
                    Category = L.T("Dashboard.Component.Ram"),
                    Name = FormatHelper.FormatBytes(ramTotal),
                    Detail = $"{FormatHelper.FormatBytes(ramUsed)} {L.T("Dashboard.Component.InUse")}",
                    SpecLine = L.T("Dashboard.Component.RamSpec"),
                    StatusText = ramLoad > 85 ? L.T("Dashboard.Component.High") : L.T("Dashboard.Component.Normal"),
                    StatusColorHex = ramLoad > 85 ? "#F59E0B" : "#10B981",
                    IconGlyph = "\uE964"
                },
                new SystemComponent
                {
                    Category = L.T("Dashboard.Component.Storage"),
                    Name = L.T("Dashboard.Component.PrimaryDrive"),
                    Detail = diskTotal > 0
                        ? $"{FormatHelper.FormatBytes(diskUsed)} / {FormatHelper.FormatBytes(diskTotal)}"
                        : L.T("Dashboard.Component.StorageUnknown"),
                    SpecLine = L.T("Dashboard.Component.StorageSpec"),
                    StatusText = diskUsage > 85 ? L.T("Dashboard.Component.High") : L.T("Dashboard.Component.Healthy"),
                    StatusColorHex = diskUsage > 85 ? "#F59E0B" : "#10B981",
                    IconGlyph = "\uEDA2"
                },
                new SystemComponent
                {
                    Category = L.T("Dashboard.Component.Motherboard"),
                    Name = boardName,
                    Detail = ShortOsName(osDescription),
                    SpecLine = $"{L.T("Dashboard.Snapshot.Uptime")}: {FormatUptimeShort(uptime)}",
                    StatusText = L.T("Dashboard.Component.Detected"),
                    IconGlyph = "\uE964"
                }
            ],
            RecommendedActions = BuildRecommendedActions(diskUsage, ramLoad)
        };

        return Task.FromResult(data);
    }

    private static SystemMetric BuildCpuMetric(int cpuUsage) => new()
    {
        Name = L.T("Dashboard.Metric.Cpu"),
        Value = cpuUsage.ToString(),
        Unit = "%",
        IconGlyph = "\uE950",
        AccentColorHex = "#22D3EE",
        Percentage = cpuUsage,
        Description = L.T("Dashboard.Metric.CpuDesc"),
        DetailText = $"{Environment.ProcessorCount} {L.T("Dashboard.Component.Cores")}",
        Sparkline = HardwareInfoHelper.BuildSparkline(cpuUsage + 11)
    };

    private static SystemMetric BuildRamMetric(int ramLoad, long ramTotal, long ramUsed) => new()
    {
        Name = L.T("Dashboard.Metric.Ram"),
        Value = ramLoad.ToString(),
        Unit = "%",
        IconGlyph = "\uE964",
        AccentColorHex = "#A78BFA",
        Percentage = ramLoad,
        Description = L.T("Dashboard.Metric.RamDesc"),
        DetailText = ramTotal > 0 ? $"{FormatHelper.FormatBytes(ramUsed)} / {FormatHelper.FormatBytes(ramTotal)}" : "--",
        Sparkline = HardwareInfoHelper.BuildSparkline(ramLoad + 23)
    };

    private static SystemMetric BuildDiskMetric(int diskUsage, long diskUsed, long diskTotal, long diskFree) => new()
    {
        Name = L.T("Dashboard.Metric.Disk"),
        Value = diskUsage.ToString(),
        Unit = "%",
        IconGlyph = "\uEDA2",
        AccentColorHex = "#FBBF24",
        Percentage = diskUsage,
        Description = L.T("Dashboard.Metric.DiskDesc"),
        DetailText = diskTotal > 0 ? $"{FormatHelper.FormatBytes(diskFree)} {L.T("Dashboard.Metric.Free")}" : "--",
        StorageUsedBytes = diskUsed,
        StorageTotalBytes = diskTotal,
        StorageFreeBytes = diskFree,
        Sparkline = HardwareInfoHelper.BuildSparkline(diskUsage + 37)
    };

    private static SystemMetric BuildNetworkMetric(int networkLoad)
    {
        var download = Random.Shared.Next(12, 48);
        var upload = Random.Shared.Next(4, 18);
        return new SystemMetric
        {
            Name = L.T("Dashboard.Metric.Network"),
            Value = networkLoad.ToString(),
            Unit = "%",
            IconGlyph = "\uE701",
            AccentColorHex = "#34D399",
            Percentage = networkLoad,
            Description = L.T("Dashboard.Metric.NetworkDesc"),
            DetailText = L.T("Dashboard.NetworkOnline"),
            DownloadText = $"{download} Mbps",
            UploadText = $"{upload} Mbps",
            Sparkline = HardwareInfoHelper.BuildSparkline(networkLoad + 51)
        };
    }

    private static IReadOnlyList<RecommendedAction> BuildRecommendedActions(int diskUsage, int ramLoad)
    {
        var reclaim = FormatHelper.FormatBytes(Random.Shared.Next(800, 4200) * 1024L * 1024L);
        var startupCount = 12;

        return
        [
            new RecommendedAction
            {
                Id = "cleanup",
                Title = L.T("Dashboard.Action.Cleanup"),
                Description = L.T("Dashboard.Action.CleanupDesc"),
                Value = reclaim,
                ActionLabel = L.T("Dashboard.Action.Review"),
                IconGlyph = "\uE74C",
                AccentColorHex = "#00D4FF",
                NavigationKey = "SmartCleaner"
            },
            new RecommendedAction
            {
                Id = "startup",
                Title = L.T("Dashboard.Action.Startup"),
                Description = L.T("Dashboard.Action.StartupDesc"),
                Value = startupCount.ToString(),
                ActionLabel = L.T("Dashboard.Action.Manage"),
                IconGlyph = "\uE7B5",
                AccentColorHex = "#A78BFA",
                NavigationKey = "Startup"
            },
            new RecommendedAction
            {
                Id = "privacy",
                Title = L.T("Dashboard.Action.Privacy"),
                Description = L.T("Dashboard.Action.PrivacyDesc"),
                Value = L.T("Dashboard.Action.PrivacyValue"),
                ActionLabel = L.T("Dashboard.Action.Check"),
                IconGlyph = "\uE72E",
                AccentColorHex = "#34D399",
                NavigationKey = "Privacy"
            }
        ];
    }

    private static int ComputeHealthScore(int cpu, int ram, int disk)
    {
        var avg = (cpu + ram + disk) / 3.0;
        return (int)Math.Clamp(Math.Round(100 - avg * 0.45 + 18), 62, 98);
    }

    private static string FormatUptime(TimeSpan uptime)
    {
        if (uptime.TotalDays >= 1)
        {
            return $"{(int)uptime.TotalDays}d {uptime.Hours}h {uptime.Minutes}m";
        }

        return $"{uptime.Hours}h {uptime.Minutes}m";
    }

    private static string FormatUptimeShort(TimeSpan uptime)
    {
        if (uptime.TotalDays >= 1)
        {
            return $"{(int)uptime.TotalDays}d {uptime.Hours}h";
        }

        return $"{uptime.Hours}h {uptime.Minutes}m";
    }

    private static string ShortOsName(string description)
    {
        if (description.Contains("Windows 11", StringComparison.OrdinalIgnoreCase))
        {
            return "Windows 11";
        }

        if (description.Contains("Windows 10", StringComparison.OrdinalIgnoreCase))
        {
            return "Windows 10";
        }

        return description.Length > 28 ? description[..28] + "…" : description;
    }
}
