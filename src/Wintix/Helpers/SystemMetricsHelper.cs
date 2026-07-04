using System.Runtime.InteropServices;

namespace Wintix.Helpers;

public static class SystemMetricsHelper
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MemoryStatusEx
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);

    public static (int LoadPercent, long TotalBytes, long UsedBytes) GetMemoryStats()
    {
        var status = new MemoryStatusEx { dwLength = (uint)Marshal.SizeOf<MemoryStatusEx>() };
        if (!GlobalMemoryStatusEx(ref status))
        {
            return (0, 0, 0);
        }

        var total = (long)status.ullTotalPhys;
        var available = (long)status.ullAvailPhys;
        var used = total - available;
        return ((int)status.dwMemoryLoad, total, used);
    }
}
