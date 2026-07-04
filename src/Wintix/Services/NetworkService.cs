using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Wintix.Models;

namespace Wintix.Services;

public sealed class NetworkService
{
    public IReadOnlyList<NetworkAdapterItem> GetAdapters()
    {
        return NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(n =>
            {
                var ipv4 = n.GetIPProperties().UnicastAddresses
                    .FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                    ?.Address.ToString() ?? "N/A";

                return new NetworkAdapterItem
                {
                    Name = n.Name,
                    Status = n.OperationalStatus.ToString(),
                    Type = n.NetworkInterfaceType.ToString(),
                    Speed = n.Speed > 0 ? $"{n.Speed / 1_000_000} Mbps" : "Unknown",
                    Ipv4 = ipv4
                };
            })
            .OrderByDescending(a => a.Status == "Up")
            .ThenBy(a => a.Name)
            .ToList();
    }

    public async Task<(bool Success, string Message, long RoundtripMs)> PingAsync(string host, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(host))
        {
            return (false, "Enter a host name or IP address.", 0);
        }

        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(host.Trim(), 4000);
            if (reply.Status == IPStatus.Success)
            {
                return (true, $"Reply from {reply.Address}: time={reply.RoundtripTime}ms", reply.RoundtripTime);
            }

            return (false, $"Ping failed: {reply.Status}", 0);
        }
        catch (Exception ex)
        {
            return (false, ex.Message, 0);
        }
    }

    public async Task<(bool Success, string Message)> FlushDnsAsync()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "ipconfig",
                Arguments = "/flushdns",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(psi);
            if (process is null)
            {
                return (false, "Could not start ipconfig.");
            }

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return process.ExitCode == 0
                ? (true, string.IsNullOrWhiteSpace(output) ? "DNS cache flushed." : output.Trim())
                : (false, "DNS flush failed.");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public bool OpenNetworkSettings()
    {
        try
        {
            Process.Start(new ProcessStartInfo("ms-settings:network") { UseShellExecute = true });
            return true;
        }
        catch
        {
            return false;
        }
    }
}
