namespace Wintix.Models;

public sealed class SystemSnapshot
{
    public required string Label { get; init; }
    public required string Value { get; init; }
    public required string IconGlyph { get; init; }
    public required string AccentColorHex { get; init; }
}
