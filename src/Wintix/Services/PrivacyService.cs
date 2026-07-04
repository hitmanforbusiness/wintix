using Microsoft.Win32;
using Wintix.Models;

namespace Wintix.Services;

public sealed class PrivacyService
{
    public IReadOnlyList<PrivacyActionItem> GetPrivacyOverview()
    {
        return
        [
            new PrivacyActionItem
            {
                Id = "telemetry",
                Title = "Diagnostic Data",
                Description = "Windows telemetry and diagnostic data sharing level.",
                Status = ReadTelemetryStatus(),
                IconGlyph = "\uE9D9"
            },
            new PrivacyActionItem
            {
                Id = "advertising",
                Title = "Advertising ID",
                Description = "Let apps use your advertising ID for experiences across apps.",
                Status = ReadAdvertisingIdStatus(),
                IconGlyph = "\uE774"
            },
            new PrivacyActionItem
            {
                Id = "location",
                Title = "Location Services",
                Description = "Allow Windows and apps to access location.",
                Status = ReadLocationStatus(),
                IconGlyph = "\uE81D"
            },
            new PrivacyActionItem
            {
                Id = "temp-clean",
                Title = "Temporary Data",
                Description = "Remove temporary files that may contain usage traces.",
                Status = "Action available",
                IconGlyph = "\uE74C"
            }
        ];
    }

    public bool OpenPrivacySettings()
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("ms-settings:privacy") { UseShellExecute = true });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool OpenLocationSettings()
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("ms-settings:privacy-location") { UseShellExecute = true });
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string ReadTelemetryStatus()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DataCollection");
            var allow = key?.GetValue("AllowTelemetry")?.ToString();
            return allow switch
            {
                "0" => "Security (minimal)",
                "1" => "Basic",
                "2" => "Enhanced",
                "3" => "Full",
                _ => "System default"
            };
        }
        catch
        {
            return "Unknown";
        }
    }

    private static string ReadAdvertisingIdStatus()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo");
            var enabled = key?.GetValue("Enabled");
            return enabled is int value && value == 0 ? "Disabled" : "Enabled";
        }
        catch
        {
            return "Unknown";
        }
    }

    private static string ReadLocationStatus()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\location");
            var value = key?.GetValue("Value")?.ToString();
            return value switch
            {
                "Allow" => "Allowed",
                "Deny" => "Denied",
                _ => "System default"
            };
        }
        catch
        {
            return "Unknown";
        }
    }
}
