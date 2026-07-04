namespace Wintix.Helpers;

public static class FormatHelper
{
    public static string FormatBytes(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB", "TB"];
        double size = bytes;
        var unit = 0;

        while (size >= 1024 && unit < units.Length - 1)
        {
            size /= 1024;
            unit++;
        }

        return unit == 0
            ? $"{bytes} B"
            : $"{size:0.##} {units[unit]}";
    }

    public static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays >= 1)
        {
            return $"{(int)duration.TotalDays}d {duration.Hours}h";
        }

        if (duration.TotalHours >= 1)
        {
            return $"{duration.Hours}h {duration.Minutes}m";
        }

        return $"{duration.Minutes}m {duration.Seconds}s";
    }
}
