namespace Wintix.Models;

public sealed class PrivacyActionItem
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Status { get; init; }
    public required string IconGlyph { get; init; }
    public bool IsActionAvailable { get; init; } = true;
    public string ManageLabel { get; init; } = string.Empty;
}
