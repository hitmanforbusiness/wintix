using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.Localization;

public sealed partial class LocalizationService : ObservableObject
{
    public static LocalizationService Instance { get; } = new();

    [ObservableProperty]
    private string _currentLanguage = "tr";

    public event EventHandler? LanguageChanged;

    private static readonly Dictionary<string, Dictionary<string, string>> Strings = new()
    {
        ["en"] = BuildEnglish(),
        ["tr"] = BuildTurkish()
    };

    private LocalizationService()
    {
    }

    public void Initialize(AppSettings? settings = null)
    {
        var language = ResolveLanguage(settings?.Language);
        SetLanguage(language, persist: false);
    }

    public string Get(string key)
    {
        if (Strings.TryGetValue(CurrentLanguage, out var lang) && lang.TryGetValue(key, out var value))
        {
            return value;
        }

        if (Strings.TryGetValue("en", out var fallback) && fallback.TryGetValue(key, out var english))
        {
            return english;
        }

        return key;
    }

    public string Get(string key, params object[] args) => string.Format(Get(key), args);

    public void SetLanguage(string language, bool persist = true)
    {
        language = language is "tr" or "en" ? language : ResolveLanguage(language);
        if (CurrentLanguage == language)
        {
            return;
        }

        CurrentLanguage = language;
        LanguageChanged?.Invoke(this, EventArgs.Empty);

        if (persist)
        {
            var settings = SettingsService.Instance.Current;
            settings.Language = language;
            SettingsService.Instance.Save(settings);
        }
    }

    public IReadOnlyList<(string Code, string Label)> GetLanguageOptions() =>
    [
        ("tr", GetForLanguage("tr", "Settings.Language.Tr")),
        ("en", GetForLanguage("en", "Settings.Language.En"))
    ];

    private string GetForLanguage(string lang, string key)
        => Strings.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out var value) ? value : key;

    private static string ResolveLanguage(string? preference)
    {
        if (preference is "tr" or "en")
        {
            return preference;
        }

        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
        return culture == "tr" ? "tr" : "en";
    }

    private static Dictionary<string, string> BuildEnglish() => new()
    {
        ["App.Name"] = "Wintix",
        ["App.Tagline"] = "SYSTEM SUITE",
        ["App.Footer"] = "Premium Windows optimization",
        ["App.SystemReady"] = "SYSTEM READY",
        ["App.Version"] = "VERSION 0.2.0",

        ["Nav.Dashboard"] = "Dashboard",
        ["Nav.SmartCleaner"] = "Smart Cleaner",
        ["Nav.Performance"] = "Performance",
        ["Nav.Privacy"] = "Privacy",
        ["Nav.Startup"] = "Startup Manager",
        ["Nav.Network"] = "Network Tools",
        ["Nav.Scheduler"] = "Scheduler",
        ["Nav.Logs"] = "Activity Logs",
        ["Nav.Settings"] = "Settings",

        ["Title.Dashboard"] = "System Overview",
        ["Title.SmartCleaner"] = "Smart Cleaner",
        ["Title.Performance"] = "Performance Monitor",
        ["Title.Privacy"] = "Privacy Shield",
        ["Title.Startup"] = "Startup Manager",
        ["Title.Network"] = "Network Tools",
        ["Title.Scheduler"] = "Task Scheduler",
        ["Title.Logs"] = "Activity Logs",
        ["Title.Settings"] = "Settings",

        ["Dashboard.Subtitle"] = "A quick look at your system health and optimization status.",
        ["Dashboard.QuickScan"] = "Run Quick Scan",
        ["Dashboard.SystemUsage"] = "System Usage",
        ["Dashboard.Components"] = "System Components",
        ["Dashboard.HealthScore"] = "HEALTH SCORE",
        ["Dashboard.Snapshot.Os"] = "OPERATING SYSTEM",
        ["Dashboard.Snapshot.Uptime"] = "UPTIME",
        ["Dashboard.Snapshot.Cpu"] = "CPU USAGE",
        ["Dashboard.Snapshot.Ram"] = "RAM USAGE",
        ["Dashboard.Snapshot.Disk"] = "DISK USAGE",
        ["Dashboard.Snapshot.Network"] = "NETWORK",
        ["Dashboard.NetworkOnline"] = "Online",
        ["Dashboard.NetworkStatus"] = "Connected · Stable",
        ["Dashboard.Optimized"] = "System is optimized. No critical issues detected.",
        ["Dashboard.Metric.Cpu"] = "CPU",
        ["Dashboard.Metric.Ram"] = "RAM",
        ["Dashboard.Metric.Disk"] = "DISK",
        ["Dashboard.Metric.Network"] = "NETWORK",
        ["Dashboard.DeepScan"] = "Deep Scan",
        ["Dashboard.LastScan"] = "Last scan",
        ["Dashboard.LastScan.Never"] = "Never scanned",
        ["Dashboard.LastScan.At"] = "Last scan: {0}",
        ["Dashboard.RecommendedActions"] = "Recommended Actions",
        ["Dashboard.Health.Excellent"] = "Excellent",
        ["Dashboard.Health.Good"] = "Good",
        ["Dashboard.Health.Fair"] = "Fair",
        ["Dashboard.Health.Attention"] = "Needs attention",
        ["Dashboard.Metric.CpuDesc"] = "Processor load across all cores",
        ["Dashboard.Metric.RamDesc"] = "Active memory consumption",
        ["Dashboard.Metric.DiskDesc"] = "Primary system drive usage",
        ["Dashboard.Metric.NetworkDesc"] = "Active network throughput",
        ["Dashboard.Metric.Free"] = "free",
        ["Dashboard.Metric.Download"] = "Download",
        ["Dashboard.Metric.Upload"] = "Upload",
        ["Dashboard.Metric.Used"] = "Used",
        ["Dashboard.Metric.Total"] = "Total",
        ["Dashboard.Action.Cleanup"] = "Cleanup Available",
        ["Dashboard.Action.CleanupDesc"] = "Temporary files and caches ready to remove",
        ["Dashboard.Action.Review"] = "Review",
        ["Dashboard.Action.Startup"] = "Startup Impact",
        ["Dashboard.Action.StartupDesc"] = "Programs affecting boot time",
        ["Dashboard.Action.Manage"] = "Manage",
        ["Dashboard.Action.Privacy"] = "Privacy Check",
        ["Dashboard.Action.PrivacyDesc"] = "Review telemetry and privacy settings",
        ["Dashboard.Action.PrivacyValue"] = "Review",
        ["Dashboard.Action.Check"] = "Check now",
        ["Dashboard.Component.Cpu"] = "PROCESSOR",
        ["Dashboard.Component.Gpu"] = "GRAPHICS",
        ["Dashboard.Component.Ram"] = "MEMORY",
        ["Dashboard.Component.Storage"] = "STORAGE",
        ["Dashboard.Component.Motherboard"] = "MOTHERBOARD",
        ["Dashboard.Component.Cores"] = "cores",
        ["Dashboard.Component.PerformanceMode"] = "Balanced power",
        ["Dashboard.Component.CpuSpec"] = "x64 · Hardware accelerated",
        ["Dashboard.Component.GpuDetail"] = "Hardware accelerated rendering",
        ["Dashboard.Component.InUse"] = "in use",
        ["Dashboard.Component.RamSpec"] = "DDR · Dual channel",
        ["Dashboard.Component.PrimaryDrive"] = "System Drive (C:)",
        ["Dashboard.Component.StorageUnknown"] = "Drive info unavailable",
        ["Dashboard.Component.StorageSpec"] = "NVMe / SSD · NTFS",
        ["Dashboard.Component.Normal"] = "Normal",
        ["Dashboard.Component.Active"] = "Active",
        ["Dashboard.Component.High"] = "High",
        ["Dashboard.Component.Healthy"] = "Healthy",
        ["Dashboard.Component.Detected"] = "Detected",

        ["Cleaner.Subtitle"] = "Find and remove temporary files, caches, and other reclaimable data.",
        ["Cleaner.ScanNow"] = "Scan Now",
        ["Cleaner.Reclaimable"] = "RECLAIMABLE SPACE",
        ["Cleaner.Scan"] = "Scan",
        ["Cleaner.CleanSelected"] = "Clean Selected",
        ["Cleaner.Categories"] = "Cleanup Categories",
        ["Cleaner.Status.Idle"] = "Scan your system to find reclaimable space.",
        ["Cleaner.Status.Scanning"] = "Scanning selected locations...",
        ["Cleaner.Status.ScanDone"] = "Scan complete. Review categories before cleaning.",
        ["Cleaner.Status.Cleaning"] = "Cleaning selected categories...",
        ["Cleaner.Status.CleanDone"] = "Cleanup finished. Reclaimed {0}.",
        ["Cleaner.Status.NoSelection"] = "Run a scan and select categories with reclaimable space.",
        ["Cleaner.Cat.temp-user.Name"] = "User Temp Files",
        ["Cleaner.Cat.temp-user.Desc"] = "Temporary files stored in your user profile.",
        ["Cleaner.Cat.temp-local.Name"] = "Local App Temp",
        ["Cleaner.Cat.temp-local.Desc"] = "Application cache and temp data.",
        ["Cleaner.Cat.recent.Name"] = "Recent Items",
        ["Cleaner.Cat.recent.Desc"] = "Recently used file shortcuts.",
        ["Cleaner.Cat.thumbnails.Name"] = "Thumbnail Cache",
        ["Cleaner.Cat.thumbnails.Desc"] = "Explorer thumbnail previews cache.",
        ["Cleaner.Cat.recycle.Name"] = "Recycle Bin",
        ["Cleaner.Cat.recycle.Desc"] = "Deleted files waiting for permanent removal.",

        ["Performance.Subtitle"] = "Monitor memory usage and inspect top running processes.",
        ["Performance.Refresh"] = "Refresh",
        ["Performance.MemoryLoad"] = "MEMORY LOAD",
        ["Performance.UsedOf"] = "Used {0} / {1}",
        ["Performance.TopProcesses"] = "Top Processes",
        ["Performance.StartMonitor"] = "Start Live Monitor",
        ["Performance.StopMonitor"] = "Stop Live Monitor",
        ["Performance.End"] = "End",
        ["Performance.Status.Idle"] = "Press refresh or start live monitoring.",

        ["Privacy.Subtitle"] = "Review privacy-related settings and run quick privacy actions.",
        ["Privacy.Refresh"] = "Refresh",
        ["Privacy.OpenSettings"] = "Windows Privacy Settings",
        ["Privacy.CleanTemp"] = "Clean Temp Data",
        ["Privacy.Manage"] = "Manage",
        ["Privacy.Overview"] = "Privacy Overview",

        ["Startup.Subtitle"] = "Manage programs that launch automatically when Windows starts.",
        ["Startup.Refresh"] = "Refresh",
        ["Startup.Disable"] = "Disable",
        ["Startup.Enabled"] = "Enabled",
        ["Startup.Entries"] = "Startup Entries",

        ["Network.Subtitle"] = "Inspect adapters, test connectivity, and flush DNS cache.",
        ["Network.Refresh"] = "Refresh",
        ["Network.PingTest"] = "Ping Test",
        ["Network.Ping"] = "Ping",
        ["Network.FlushDns"] = "Flush DNS",
        ["Network.OpenSettings"] = "Open Network Settings",
        ["Network.Adapters"] = "Network Adapters",
        ["Network.HostPlaceholder"] = "Host or IP (e.g. 1.1.1.1)",

        ["Scheduler.Subtitle"] = "View Windows scheduled tasks and open Task Scheduler.",
        ["Scheduler.LoadTasks"] = "Load Tasks",
        ["Scheduler.Open"] = "Open Task Scheduler",
        ["Scheduler.Run"] = "Run",
        ["Scheduler.Tasks"] = "Scheduled Tasks",

        ["Logs.Subtitle"] = "Track scans, cleanups, network actions, and system changes.",
        ["Logs.Clear"] = "Clear Log",
        ["Logs.FilterPlaceholder"] = "Filter by category or message...",
        ["Logs.Refresh"] = "Refresh",
        ["Logs.Entries"] = "Log Entries",

        ["Settings.Subtitle"] = "Configure Wintix preferences, appearance, and update behavior.",
        ["Settings.Appearance"] = "Appearance",
        ["Settings.DarkTheme"] = "Use dark theme",
        ["Settings.AccentGlow"] = "Accent glow effects",
        ["Settings.Notifications"] = "Notifications",
        ["Settings.HealthAlerts"] = "System health alerts",
        ["Settings.ScanReminders"] = "Scheduled scan reminders",
        ["Settings.StartupSection"] = "Startup",
        ["Settings.LaunchAtStartup"] = "Launch Wintix at Windows startup",
        ["Settings.LanguageSection"] = "Language",
        ["Settings.Language"] = "Application language",
        ["Settings.Language.Tr"] = "Turkish",
        ["Settings.Language.En"] = "English",
        ["Settings.Save"] = "Save Settings",
        ["Settings.Reset"] = "Reset",
        ["Settings.About"] = "About",
        ["Settings.AboutText"] = "Wintix — Premium Windows optimization suite",
        ["Settings.Saved"] = "Settings saved successfully.",

        ["Common.Online"] = "Online",
        ["Common.Running"] = "Running",
        ["Common.NotResponding"] = "Not responding",
        ["Common.ComingSoon"] = "COMING SOON",
    };

    private static Dictionary<string, string> BuildTurkish() => new()
    {
        ["App.Name"] = "Wintix",
        ["App.Tagline"] = "SİSTEM PAKETİ",
        ["App.Footer"] = "Premium Windows optimizasyonu",
        ["App.SystemReady"] = "SİSTEM HAZIR",
        ["App.Version"] = "SÜRÜM 0.2.0",

        ["Nav.Dashboard"] = "Kontrol Paneli",
        ["Nav.SmartCleaner"] = "Akıllı Temizleyici",
        ["Nav.Performance"] = "Performans",
        ["Nav.Privacy"] = "Gizlilik",
        ["Nav.Startup"] = "Başlangıç Yöneticisi",
        ["Nav.Network"] = "Ağ Araçları",
        ["Nav.Scheduler"] = "Zamanlayıcı",
        ["Nav.Logs"] = "Etkinlik Günlükleri",
        ["Nav.Settings"] = "Ayarlar",

        ["Title.Dashboard"] = "Sistem Özeti",
        ["Title.SmartCleaner"] = "Akıllı Temizleyici",
        ["Title.Performance"] = "Performans Monitörü",
        ["Title.Privacy"] = "Gizlilik Kalkanı",
        ["Title.Startup"] = "Başlangıç Yöneticisi",
        ["Title.Network"] = "Ağ Araçları",
        ["Title.Scheduler"] = "Görev Zamanlayıcı",
        ["Title.Logs"] = "Etkinlik Günlükleri",
        ["Title.Settings"] = "Ayarlar",

        ["Dashboard.Subtitle"] = "Sistem sağlığınız ve optimizasyon durumunuza hızlı bir bakış.",
        ["Dashboard.QuickScan"] = "Hızlı Tarama",
        ["Dashboard.SystemUsage"] = "Sistem Kullanımı",
        ["Dashboard.Components"] = "Sistem Bileşenleri",
        ["Dashboard.HealthScore"] = "SAĞLIK SKORU",
        ["Dashboard.Snapshot.Os"] = "İŞLETİM SİSTEMİ",
        ["Dashboard.Snapshot.Uptime"] = "ÇALIŞMA SÜRESİ",
        ["Dashboard.Snapshot.Cpu"] = "CPU KULLANIMI",
        ["Dashboard.Snapshot.Ram"] = "RAM KULLANIMI",
        ["Dashboard.Snapshot.Disk"] = "DİSK KULLANIMI",
        ["Dashboard.Snapshot.Network"] = "AĞ",
        ["Dashboard.NetworkOnline"] = "Çevrimiçi",
        ["Dashboard.NetworkStatus"] = "Bağlı · Stabil",
        ["Dashboard.Optimized"] = "Sistem optimize. Kritik sorun tespit edilmedi.",
        ["Dashboard.Metric.Cpu"] = "CPU",
        ["Dashboard.Metric.Ram"] = "RAM",
        ["Dashboard.Metric.Disk"] = "DİSK",
        ["Dashboard.Metric.Network"] = "AĞ",
        ["Dashboard.DeepScan"] = "Derin Tarama",
        ["Dashboard.LastScan"] = "Son tarama",
        ["Dashboard.LastScan.Never"] = "Henüz taranmadı",
        ["Dashboard.LastScan.At"] = "Son tarama: {0}",
        ["Dashboard.RecommendedActions"] = "Önerilen İşlemler",
        ["Dashboard.Health.Excellent"] = "Mükemmel",
        ["Dashboard.Health.Good"] = "İyi",
        ["Dashboard.Health.Fair"] = "Orta",
        ["Dashboard.Health.Attention"] = "Dikkat gerekli",
        ["Dashboard.Metric.CpuDesc"] = "Tüm çekirdeklerde işlemci yükü",
        ["Dashboard.Metric.RamDesc"] = "Aktif bellek tüketimi",
        ["Dashboard.Metric.DiskDesc"] = "Birincil sistem sürücüsü kullanımı",
        ["Dashboard.Metric.NetworkDesc"] = "Aktif ağ trafiği",
        ["Dashboard.Metric.Free"] = "boş",
        ["Dashboard.Metric.Download"] = "İndirme",
        ["Dashboard.Metric.Upload"] = "Yükleme",
        ["Dashboard.Metric.Used"] = "Kullanılan",
        ["Dashboard.Metric.Total"] = "Toplam",
        ["Dashboard.Action.Cleanup"] = "Temizlik Mevcut",
        ["Dashboard.Action.CleanupDesc"] = "Kaldırılmaya hazır geçici dosyalar ve önbellekler",
        ["Dashboard.Action.Review"] = "İncele",
        ["Dashboard.Action.Startup"] = "Başlangıç Etkisi",
        ["Dashboard.Action.StartupDesc"] = "Açılış süresini etkileyen programlar",
        ["Dashboard.Action.Manage"] = "Yönet",
        ["Dashboard.Action.Privacy"] = "Gizlilik Kontrolü",
        ["Dashboard.Action.PrivacyDesc"] = "Telemetri ve gizlilik ayarlarını gözden geçirin",
        ["Dashboard.Action.PrivacyValue"] = "İncele",
        ["Dashboard.Action.Check"] = "Kontrol et",
        ["Dashboard.Component.Cpu"] = "İŞLEMCİ",
        ["Dashboard.Component.Gpu"] = "GRAFİK",
        ["Dashboard.Component.Ram"] = "BELLEK",
        ["Dashboard.Component.Storage"] = "DEPOLAMA",
        ["Dashboard.Component.Motherboard"] = "ANAKART",
        ["Dashboard.Component.Cores"] = "çekirdek",
        ["Dashboard.Component.PerformanceMode"] = "Dengeli güç",
        ["Dashboard.Component.CpuSpec"] = "x64 · Donanım hızlandırmalı",
        ["Dashboard.Component.GpuDetail"] = "Donanım hızlandırmalı görüntüleme",
        ["Dashboard.Component.InUse"] = "kullanımda",
        ["Dashboard.Component.RamSpec"] = "DDR · Çift kanal",
        ["Dashboard.Component.PrimaryDrive"] = "Sistem Sürücüsü (C:)",
        ["Dashboard.Component.StorageUnknown"] = "Sürücü bilgisi alınamadı",
        ["Dashboard.Component.StorageSpec"] = "NVMe / SSD · NTFS",
        ["Dashboard.Component.Normal"] = "Normal",
        ["Dashboard.Component.Active"] = "Aktif",
        ["Dashboard.Component.High"] = "Yüksek",
        ["Dashboard.Component.Healthy"] = "Sağlıklı",
        ["Dashboard.Component.Detected"] = "Algılandı",

        ["Cleaner.Subtitle"] = "Geçici dosyaları, önbellekleri ve geri kazanılabilir verileri bulun ve temizleyin.",
        ["Cleaner.ScanNow"] = "Şimdi Tara",
        ["Cleaner.Reclaimable"] = "GERİ KAZANILABİLİR ALAN",
        ["Cleaner.Scan"] = "Tara",
        ["Cleaner.CleanSelected"] = "Seçilenleri Temizle",
        ["Cleaner.Categories"] = "Temizlik Kategorileri",
        ["Cleaner.Status.Idle"] = "Geri kazanılabilir alanı bulmak için sisteminizi tarayın.",
        ["Cleaner.Status.Scanning"] = "Seçili konumlar taranıyor...",
        ["Cleaner.Status.ScanDone"] = "Tarama tamamlandı. Temizlemeden önce kategorileri gözden geçirin.",
        ["Cleaner.Status.Cleaning"] = "Seçili kategoriler temizleniyor...",
        ["Cleaner.Status.CleanDone"] = "Temizlik tamamlandı. {0} geri kazanıldı.",
        ["Cleaner.Status.NoSelection"] = "Tarama yapın ve geri kazanılabilir alanı olan kategorileri seçin.",
        ["Cleaner.Cat.temp-user.Name"] = "Kullanıcı Geçici Dosyaları",
        ["Cleaner.Cat.temp-user.Desc"] = "Kullanıcı profilinizde saklanan geçici dosyalar.",
        ["Cleaner.Cat.temp-local.Name"] = "Yerel Uygulama Temp",
        ["Cleaner.Cat.temp-local.Desc"] = "Uygulama önbelleği ve geçici veriler.",
        ["Cleaner.Cat.recent.Name"] = "Son Kullanılanlar",
        ["Cleaner.Cat.recent.Desc"] = "Son kullanılan dosya kısayolları.",
        ["Cleaner.Cat.thumbnails.Name"] = "Küçük Resim Önbelleği",
        ["Cleaner.Cat.thumbnails.Desc"] = "Gezgin küçük resim önbelleği.",
        ["Cleaner.Cat.recycle.Name"] = "Geri Dönüşüm Kutusu",
        ["Cleaner.Cat.recycle.Desc"] = "Kalıcı silinmeyi bekleyen dosyalar.",

        ["Performance.Subtitle"] = "Bellek kullanımını izleyin ve en çok kaynak tüketen süreçleri inceleyin.",
        ["Performance.Refresh"] = "Yenile",
        ["Performance.MemoryLoad"] = "BELLEK YÜKÜ",
        ["Performance.UsedOf"] = "Kullanılan {0} / {1}",
        ["Performance.TopProcesses"] = "En Yüksek Süreçler",
        ["Performance.StartMonitor"] = "Canlı İzlemeyi Başlat",
        ["Performance.StopMonitor"] = "Canlı İzlemeyi Durdur",
        ["Performance.End"] = "Sonlandır",
        ["Performance.Status.Idle"] = "Yenile'ye basın veya canlı izlemeyi başlatın.",

        ["Privacy.Subtitle"] = "Gizlilik ayarlarını inceleyin ve hızlı gizlilik işlemleri çalıştırın.",
        ["Privacy.Refresh"] = "Yenile",
        ["Privacy.OpenSettings"] = "Windows Gizlilik Ayarları",
        ["Privacy.CleanTemp"] = "Geçici Verileri Temizle",
        ["Privacy.Manage"] = "Yönet",
        ["Privacy.Overview"] = "Gizlilik Özeti",

        ["Startup.Subtitle"] = "Windows başladığında otomatik açılan programları yönetin.",
        ["Startup.Refresh"] = "Yenile",
        ["Startup.Disable"] = "Devre Dışı Bırak",
        ["Startup.Enabled"] = "Etkin",
        ["Startup.Entries"] = "Başlangıç Girdileri",

        ["Network.Subtitle"] = "Adaptörleri inceleyin, bağlantıyı test edin ve DNS önbelleğini temizleyin.",
        ["Network.Refresh"] = "Yenile",
        ["Network.PingTest"] = "Ping Testi",
        ["Network.Ping"] = "Ping",
        ["Network.FlushDns"] = "DNS Temizle",
        ["Network.OpenSettings"] = "Ağ Ayarlarını Aç",
        ["Network.Adapters"] = "Ağ Adaptörleri",
        ["Network.HostPlaceholder"] = "Sunucu veya IP (örn. 1.1.1.1)",

        ["Scheduler.Subtitle"] = "Windows zamanlanmış görevlerini görüntüleyin ve Görev Zamanlayıcı'yı açın.",
        ["Scheduler.LoadTasks"] = "Görevleri Yükle",
        ["Scheduler.Open"] = "Görev Zamanlayıcı'yı Aç",
        ["Scheduler.Run"] = "Çalıştır",
        ["Scheduler.Tasks"] = "Zamanlanmış Görevler",

        ["Logs.Subtitle"] = "Taramaları, temizlikleri, ağ işlemlerini ve sistem değişikliklerini takip edin.",
        ["Logs.Clear"] = "Günlüğü Temizle",
        ["Logs.FilterPlaceholder"] = "Kategori veya mesaja göre filtrele...",
        ["Logs.Refresh"] = "Yenile",
        ["Logs.Entries"] = "Günlük Kayıtları",

        ["Settings.Subtitle"] = "Wintix tercihlerini, görünümü ve güncelleme davranışını yapılandırın.",
        ["Settings.Appearance"] = "Görünüm",
        ["Settings.DarkTheme"] = "Koyu tema kullan",
        ["Settings.AccentGlow"] = "Vurgu parıltı efektleri",
        ["Settings.Notifications"] = "Bildirimler",
        ["Settings.HealthAlerts"] = "Sistem sağlığı uyarıları",
        ["Settings.ScanReminders"] = "Zamanlanmış tarama hatırlatıcıları",
        ["Settings.StartupSection"] = "Başlangıç",
        ["Settings.LaunchAtStartup"] = "Windows başlangıcında Wintix'i aç",
        ["Settings.LanguageSection"] = "Dil",
        ["Settings.Language"] = "Uygulama dili",
        ["Settings.Language.Tr"] = "Türkçe",
        ["Settings.Language.En"] = "English",
        ["Settings.Save"] = "Ayarları Kaydet",
        ["Settings.Reset"] = "Sıfırla",
        ["Settings.About"] = "Hakkında",
        ["Settings.AboutText"] = "Wintix — Premium Windows optimizasyon paketi",
        ["Settings.Saved"] = "Ayarlar başarıyla kaydedildi.",

        ["Common.Online"] = "Çevrimiçi",
        ["Common.Running"] = "Çalışıyor",
        ["Common.NotResponding"] = "Yanıt vermiyor",
        ["Common.ComingSoon"] = "YAKINDA",
    };
}

public static class L
{
    public static string T(string key) => LocalizationService.Instance.Get(key);
    public static string T(string key, params object[] args) => LocalizationService.Instance.Get(key, args);
}
