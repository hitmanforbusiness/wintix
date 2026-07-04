using System.Text.Json;
using Wintix.Models;

namespace Wintix.Services;

public sealed class SettingsService
{
    public static SettingsService Instance { get; } = new();

    private readonly string _settingsPath;
    private readonly object _sync = new();

    public AppSettings Current { get; private set; } = new();

    private SettingsService()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Wintix");
        Directory.CreateDirectory(folder);
        _settingsPath = Path.Combine(folder, "settings.json");
        Load();
    }

    public void Load()
    {
        lock (_sync)
        {
            if (!File.Exists(_settingsPath))
            {
                Current = new AppSettings();
                return;
            }

            try
            {
                var json = File.ReadAllText(_settingsPath);
                Current = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                Current = new AppSettings();
            }
        }
    }

    public void Save(AppSettings settings)
    {
        lock (_sync)
        {
            Current = settings;
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsPath, json);
        }

        ActivityLogService.Instance.Info("Settings", "Preferences saved.");
    }
}
