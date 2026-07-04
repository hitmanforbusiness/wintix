using System.Collections.ObjectModel;
using System.Text.Json;
using Wintix.Models;

namespace Wintix.Services;

public sealed class ActivityLogService
{
    public static ActivityLogService Instance { get; } = new();

    private readonly string _logFilePath;
    private readonly object _sync = new();

    public ObservableCollection<ActivityLogEntry> Entries { get; } = [];

    private ActivityLogService()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Wintix");
        Directory.CreateDirectory(folder);
        _logFilePath = Path.Combine(folder, "activity-log.json");
        LoadFromDisk();
    }

    public void Info(string category, string message) => Add(ActivityLogLevel.Info, category, message);
    public void Success(string category, string message) => Add(ActivityLogLevel.Success, category, message);
    public void Warning(string category, string message) => Add(ActivityLogLevel.Warning, category, message);
    public void Error(string category, string message) => Add(ActivityLogLevel.Error, category, message);

    public void Add(ActivityLogLevel level, string category, string message)
    {
        var entry = new ActivityLogEntry
        {
            Timestamp = DateTimeOffset.Now,
            Category = category,
            Message = message,
            Level = level
        };

        lock (_sync)
        {
            Entries.Insert(0, entry);
            while (Entries.Count > 500)
            {
                Entries.RemoveAt(Entries.Count - 1);
            }

            SaveToDisk();
        }
    }

    public void Clear()
    {
        lock (_sync)
        {
            Entries.Clear();
            SaveToDisk();
        }
    }

    private void LoadFromDisk()
    {
        if (!File.Exists(_logFilePath))
        {
            return;
        }

        try
        {
            var json = File.ReadAllText(_logFilePath);
            var items = JsonSerializer.Deserialize<List<ActivityLogEntry>>(json);
            if (items is null)
            {
                return;
            }

            foreach (var item in items.OrderByDescending(e => e.Timestamp).Take(500))
            {
                Entries.Add(item);
            }
        }
        catch
        {
            // Ignore corrupt log files.
        }
    }

    private void SaveToDisk()
    {
        try
        {
            var json = JsonSerializer.Serialize(Entries.Take(500).Reverse().ToList());
            File.WriteAllText(_logFilePath, json);
        }
        catch
        {
            // Ignore persistence failures.
        }
    }
}
