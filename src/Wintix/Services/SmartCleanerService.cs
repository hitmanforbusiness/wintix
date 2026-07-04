using Wintix.Helpers;
using Wintix.Models;

namespace Wintix.Services;

public sealed class SmartCleanerService
{
    public IReadOnlyList<CleanCategory> CreateDefaultCategories()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        return
        [
            new CleanCategory
            {
                Id = "temp-user",
                Path = Path.GetTempPath()
            },
            new CleanCategory
            {
                Id = "temp-local",
                Path = Path.Combine(localAppData, "Temp")
            },
            new CleanCategory
            {
                Id = "recent",
                Path = Path.Combine(appData, "Microsoft", "Windows", "Recent")
            },
            new CleanCategory
            {
                Id = "thumbnails",
                Path = Path.Combine(localAppData, "Microsoft", "Windows", "Explorer")
            },
            new CleanCategory
            {
                Id = "recycle",
                Path = string.Empty
            }
        ];
    }

    public async Task ScanAsync(IList<CleanCategory> categories, CancellationToken cancellationToken = default)
    {
        foreach (var category in categories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (category.Id == "recycle")
            {
                category.SizeBytes = await Task.Run(GetRecycleBinSizeEstimate, cancellationToken);
                continue;
            }

            if (!Directory.Exists(category.Path))
            {
                category.SizeBytes = 0;
                continue;
            }

            category.SizeBytes = await GetDirectorySizeAsync(category.Path, cancellationToken);
        }
    }

    public async Task<long> CleanAsync(IEnumerable<CleanCategory> categories, CancellationToken cancellationToken = default)
    {
        long cleaned = 0;

        foreach (var category in categories.Where(c => c.IsSelected))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (category.Id == "recycle")
            {
                cleaned += await Task.Run(EmptyRecycleBin, cancellationToken);
                category.SizeBytes = 0;
                continue;
            }

            if (!Directory.Exists(category.Path))
            {
                continue;
            }

            cleaned += await CleanDirectoryAsync(category.Path, cancellationToken);
            category.SizeBytes = 0;
        }

        return cleaned;
    }

    private static long GetRecycleBinSizeEstimate()
    {
        try
        {
            long total = 0;
            var recycleRoot = new DirectoryInfo(@"C:\$Recycle.Bin");
            if (!recycleRoot.Exists)
            {
                return 0;
            }

            foreach (var file in recycleRoot.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                try
                {
                    total += file.Length;
                }
                catch
                {
                    // Skip locked entries.
                }
            }

            return total;
        }
        catch
        {
            return 0;
        }
    }

    private static long EmptyRecycleBin()
    {
        try
        {
            const int SHERB_NOCONFIRMATION = 0x00000001;
            const int SHERB_NOPROGRESSUI = 0x00000002;
            _ = SHEmptyRecycleBin(IntPtr.Zero, string.Empty, SHERB_NOCONFIRMATION | SHERB_NOPROGRESSUI);
            return 0;
        }
        catch
        {
            return 0;
        }
    }

    [System.Runtime.InteropServices.DllImport("Shell32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
    private static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, int dwFlags);

    private static async Task<long> GetDirectorySizeAsync(string path, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            long size = 0;
            try
            {
                foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    try
                    {
                        size += new FileInfo(file).Length;
                    }
                    catch
                    {
                        // Skip inaccessible files.
                    }
                }
            }
            catch
            {
                // Skip inaccessible directories.
            }

            return size;
        }, cancellationToken);
    }

    private static async Task<long> CleanDirectoryAsync(string path, CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            long cleaned = 0;
            try
            {
                foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    try
                    {
                        var info = new FileInfo(file);
                        cleaned += info.Length;
                        info.Delete();
                    }
                    catch
                    {
                        // Skip locked files.
                    }
                }

                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    try
                    {
                        cleaned += DeleteDirectoryRecursive(dir);
                    }
                    catch
                    {
                        // Skip locked folders.
                    }
                }
            }
            catch
            {
                // Skip inaccessible paths.
            }

            return cleaned;
        }, cancellationToken);
    }

    private static long DeleteDirectoryRecursive(string path)
    {
        long cleaned = 0;
        foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
        {
            try
            {
                var info = new FileInfo(file);
                cleaned += info.Length;
                info.Delete();
            }
            catch
            {
                // Skip locked files.
            }
        }

        try
        {
            Directory.Delete(path, true);
        }
        catch
        {
            // Partial cleanup is acceptable.
        }

        return cleaned;
    }
}
