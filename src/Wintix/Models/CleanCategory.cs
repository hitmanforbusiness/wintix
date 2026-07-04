namespace Wintix.Models;

public sealed class CleanCategory
{
    public required string Id { get; init; }
    public required string Path { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsSelected { get; set; } = true;
    public long SizeBytes { get; set; }

    public string FormattedSize => Helpers.FormatHelper.FormatBytes(SizeBytes);
}
