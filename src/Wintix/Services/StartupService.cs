using Microsoft.Win32;
using Wintix.Models;

namespace Wintix.Services;

public sealed class StartupService
{
    private static readonly (RegistryKey Root, string Hive)[] Sources =
    [
        (Registry.CurrentUser, "Current User"),
        (Registry.LocalMachine, "Local Machine")
    ];

    public IReadOnlyList<StartupItem> GetStartupItems()
    {
        var items = new List<StartupItem>();

        foreach (var (root, hive) in Sources)
        {
            items.AddRange(ReadRunKey(root, hive, @"Software\Microsoft\Windows\CurrentVersion\Run"));
            items.AddRange(ReadRunKey(root, hive, @"Software\Microsoft\Windows\CurrentVersion\RunOnce"));
        }

        return items
            .OrderBy(i => i.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public bool DisableStartupItem(StartupItem item, out string message)
    {
        try
        {
            var root = item.Hive == "Current User" ? Registry.CurrentUser : Registry.LocalMachine;
            using var key = root.OpenSubKey(item.RegistryKey, writable: true);
            if (key is null)
            {
                message = "Registry key not found.";
                return false;
            }

            key.DeleteValue(item.ValueName, throwOnMissingValue: false);
            item.IsEnabled = false;
            message = $"'{item.Name}' removed from startup.";
            return true;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return false;
        }
    }

    private static IEnumerable<StartupItem> ReadRunKey(RegistryKey root, string hive, string subKeyPath)
    {
        using var key = root.OpenSubKey(subKeyPath, writable: false);
        if (key is null)
        {
            yield break;
        }

        foreach (var valueName in key.GetValueNames())
        {
            if (string.IsNullOrWhiteSpace(valueName))
            {
                continue;
            }

            var command = key.GetValue(valueName)?.ToString();
            if (string.IsNullOrWhiteSpace(command))
            {
                continue;
            }

            yield return new StartupItem
            {
                Name = valueName,
                Command = command,
                Hive = hive,
                RegistryKey = subKeyPath,
                ValueName = valueName,
                IsEnabled = true
            };
        }
    }
}
