namespace Wintix.Models;

public sealed class RecommendedAction
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Value { get; init; }
    public required string ActionLabel { get; init; }
    public required string IconGlyph { get; init; }
    public required string AccentColorHex { get; init; }
    public required string NavigationKey { get; init; }
}
