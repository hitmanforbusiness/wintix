namespace Wintix.Models;

public enum ActivityLogLevel
{
    Info,
    Success,
    Warning,
    Error
}

public sealed class ActivityLogEntry
{
    public required DateTimeOffset Timestamp { get; init; }
    public required string Category { get; init; }
    public required string Message { get; init; }
    public ActivityLogLevel Level { get; init; } = ActivityLogLevel.Info;

    public string FormattedTime => Timestamp.ToString("HH:mm:ss");
    public string LevelLabel => Level.ToString().ToUpperInvariant();
}
