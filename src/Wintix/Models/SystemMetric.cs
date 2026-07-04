namespace Wintix.Models;

public sealed class SystemMetric
{
    public required string Name { get; init; }
    public required string Value { get; init; }
    public required string Unit { get; init; }
    public required string IconGlyph { get; init; }
    public required string AccentColorHex { get; init; }
    public double Percentage { get; init; }
    public string Description { get; init; } = string.Empty;
    public string DetailText { get; init; } = string.Empty;
    public string DownloadText { get; init; } = string.Empty;
    public string UploadText { get; init; } = string.Empty;
    public long StorageUsedBytes { get; init; }
    public long StorageTotalBytes { get; init; }
    public long StorageFreeBytes { get; init; }
    public IReadOnlyList<double> Sparkline { get; init; } = [];
}
