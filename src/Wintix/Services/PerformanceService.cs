using System.Diagnostics;
using Wintix.Helpers;
using Wintix.Models;

namespace Wintix.Services;

public sealed class PerformanceService
{
    public (int MemoryLoadPercent, string TotalMemory, string UsedMemory) GetMemorySnapshot()
    {
        var (load, total, used) = SystemMetricsHelper.GetMemoryStats();
        return (load, FormatHelper.FormatBytes(total), FormatHelper.FormatBytes(used));
    }

    public IReadOnlyList<ProcessInfoItem> GetTopProcesses(int count = 12)
    {
        var processes = Process.GetProcesses()
            .Select(p =>
            {
                try
                {
                    return new ProcessInfoItem
                    {
                        ProcessId = p.Id,
                        Name = p.ProcessName,
                        MemoryBytes = p.WorkingSet64,
                        FormattedMemory = FormatHelper.FormatBytes(p.WorkingSet64),
                        Status = p.Responding ? "Running" : "Not responding"
                    };
                }
                catch
                {
                    return null;
                }
                finally
                {
                    p.Dispose();
                }
            })
            .Where(p => p is not null)
            .Cast<ProcessInfoItem>()
            .OrderByDescending(p => p.MemoryBytes)
            .Take(count)
            .ToList();

        return processes;
    }

    public bool TryKillProcess(int processId, out string message)
    {
        try
        {
            using var process = Process.GetProcessById(processId);
            var name = process.ProcessName;
            process.Kill(true);
            message = $"Process '{name}' (PID {processId}) was terminated.";
            return true;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return false;
        }
    }
}
