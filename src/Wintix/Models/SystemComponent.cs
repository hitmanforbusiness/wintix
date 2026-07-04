namespace Wintix.Models;

public sealed class SystemComponent
{
    public required string Category { get; init; }
    public required string Name { get; init; }
    public required string Detail { get; init; }
    public required string IconGlyph { get; init; }
    public string SpecLine { get; init; } = string.Empty;
    public string StatusText { get; init; } = string.Empty;
    public string StatusColorHex { get; init; } = "#10B981";
}
