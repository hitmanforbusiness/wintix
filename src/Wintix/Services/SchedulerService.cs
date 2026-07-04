using System.Diagnostics;
using Wintix.Models;

namespace Wintix.Services;

public sealed class SchedulerService
{
    public async Task<IReadOnlyList<ScheduledTaskItem>> GetTasksAsync(CancellationToken cancellationToken = default)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "schtasks",
            Arguments = "/Query /FO CSV /NH",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using var process = Process.Start(psi);
        if (process is null)
        {
            return [];
        }

        var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
        await process.WaitForExitAsync(cancellationToken);

        return ParseCsvOutput(output)
            .OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase)
            .Take(100)
            .ToList();
    }

    public bool OpenTaskScheduler()
    {
        try
        {
            Process.Start(new ProcessStartInfo("taskschd.msc") { UseShellExecute = true });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RunTaskNow(string taskName, out string message)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = $"/Run /TN \"{taskName}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(psi);
            if (process is null)
            {
                message = "Could not start schtasks.";
                return false;
            }

            process.WaitForExit();
            message = process.ExitCode == 0
                ? $"Task '{taskName}' started."
                : "Failed to run task. Administrator rights may be required.";
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return false;
        }
    }

    private static IEnumerable<ScheduledTaskItem> ParseCsvOutput(string output)
    {
        using var reader = new StringReader(output);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = SplitCsvLine(line);
            if (parts.Count < 3)
            {
                continue;
            }

            yield return new ScheduledTaskItem
            {
                Name = parts[0].Trim('"'),
                NextRun = parts.Count > 1 ? parts[1].Trim('"') : "N/A",
                Status = parts.Count > 2 ? parts[2].Trim('"') : "Unknown",
                Schedule = parts.Count > 3 ? parts[3].Trim('"') : "N/A"
            };
        }
    }

    private static List<string> SplitCsvLine(string line)
    {
        var parts = new List<string>();
        var current = new System.Text.StringBuilder();
        var inQuotes = false;

        foreach (var ch in line)
        {
            if (ch == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (ch == ',' && !inQuotes)
            {
                parts.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(ch);
        }

        parts.Add(current.ToString());
        return parts;
    }
}
