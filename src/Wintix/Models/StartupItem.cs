namespace Wintix.Models;

public sealed class StartupItem
{
    public required string Name { get; init; }
    public required string Command { get; init; }
    public required string Hive { get; init; }
    public required string RegistryKey { get; init; }
    public required string ValueName { get; init; }
    public bool IsEnabled { get; set; } = true;
    public string EnabledLabel { get; init; } = string.Empty;
    public string DisableLabel { get; init; } = string.Empty;
}
