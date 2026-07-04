namespace Wintix.Models;

public sealed class ScheduledTaskItem
{
    public required string Name { get; init; }
    public required string Status { get; init; }
    public required string NextRun { get; init; }
    public required string Schedule { get; init; }
    public string RunLabel { get; init; } = string.Empty;
}
