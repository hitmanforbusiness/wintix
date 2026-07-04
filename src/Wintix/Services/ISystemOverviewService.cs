using Wintix.Models;

namespace Wintix.Services;

public interface ISystemOverviewService
{
    Task<SystemOverviewData> GetOverviewAsync(CancellationToken cancellationToken = default);
}

public sealed class SystemOverviewData
{
    public required string OperatingSystem { get; init; }
    public required string Uptime { get; init; }
    public required string CpuSummary { get; init; }
    public required string RamSummary { get; init; }
    public required string DiskSummary { get; init; }
    public required string NetworkStatus { get; init; }
    public required IReadOnlyList<SystemMetric> UsageMetrics { get; init; }
    public required IReadOnlyList<SystemSnapshot> Snapshots { get; init; }
    public required IReadOnlyList<SystemComponent> Components { get; init; }
    public required string HealthScore { get; init; }
    public required string HealthStatus { get; init; }
    public required string LastScanSummary { get; init; }
    public required string LastScanTime { get; init; }
    public required IReadOnlyList<RecommendedAction> RecommendedActions { get; init; }
    public SystemMetric? CpuMetric { get; init; }
    public SystemMetric? RamMetric { get; init; }
    public SystemMetric? DiskMetric { get; init; }
    public SystemMetric? NetworkMetric { get; init; }
}
