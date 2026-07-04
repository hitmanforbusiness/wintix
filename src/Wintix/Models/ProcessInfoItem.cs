namespace Wintix.Models;

public sealed class ProcessInfoItem
{
    public required int ProcessId { get; init; }
    public required string Name { get; init; }
    public required long MemoryBytes { get; init; }
    public required string FormattedMemory { get; init; }
    public required string Status { get; init; }
    public string ActionLabel { get; init; } = string.Empty;
}
