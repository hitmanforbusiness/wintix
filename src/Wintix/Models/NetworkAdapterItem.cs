namespace Wintix.Models;

public sealed class NetworkAdapterItem
{
    public required string Name { get; init; }
    public required string Status { get; init; }
    public required string Type { get; init; }
    public required string Speed { get; init; }
    public required string Ipv4 { get; init; }
}
