namespace Wintix.Models;

public sealed class AppSettings
{
    public string Language { get; set; } = "auto";
    public bool UseDarkTheme { get; set; } = true;
    public bool AccentGlowEffects { get; set; } = true;
    public bool SystemHealthAlerts { get; set; } = true;
    public bool ScheduledScanReminders { get; set; } = true;
    public bool LaunchAtStartup { get; set; }
    public int AutoScanIntervalHours { get; set; } = 24;
}
