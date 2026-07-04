using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class NetworkViewModel : LocalizedViewModelBase
{
    private readonly NetworkService _networkService = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _pingHost = "1.1.1.1";

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private string _pingResult = string.Empty;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private string _pageSubtitle = string.Empty;

    [ObservableProperty]
    private string _refreshActionText = string.Empty;

    [ObservableProperty]
    private string _pingTestTitle = string.Empty;

    [ObservableProperty]
    private string _pingButtonText = string.Empty;

    [ObservableProperty]
    private string _flushDnsText = string.Empty;

    [ObservableProperty]
    private string _openSettingsText = string.Empty;

    [ObservableProperty]
    private string _adaptersTitle = string.Empty;

    [ObservableProperty]
    private string _hostPlaceholder = string.Empty;

    public ObservableCollection<NetworkAdapterItem> Adapters { get; } = [];

    public NetworkViewModel() => RefreshLocalizedStrings();

    protected override void RefreshLocalizedStrings()
    {
        PageTitle = T("Title.Network");
        PageSubtitle = T("Network.Subtitle");
        RefreshActionText = T("Network.Refresh");
        PingTestTitle = T("Network.PingTest");
        PingButtonText = T("Network.Ping");
        FlushDnsText = T("Network.FlushDns");
        OpenSettingsText = T("Network.OpenSettings");
        AdaptersTitle = T("Network.Adapters");
        HostPlaceholder = T("Network.HostPlaceholder");
    }

    [RelayCommand]
    public void RefreshAdapters()
    {
        Adapters.Clear();
        foreach (var adapter in _networkService.GetAdapters())
        {
            Adapters.Add(adapter);
        }

        StatusMessage = $"{Adapters.Count} adapters";
        ActivityLogService.Instance.Info("Network Tools", StatusMessage);
    }

    [RelayCommand]
    public async Task PingAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var (success, message, _) = await _networkService.PingAsync(PingHost);
            PingResult = message;
            ActivityLogService.Instance.Add(success ? ActivityLogLevel.Success : ActivityLogLevel.Warning, "Network Tools", message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task FlushDnsAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var (success, message) = await _networkService.FlushDnsAsync();
            StatusMessage = message;
            ActivityLogService.Instance.Add(success ? ActivityLogLevel.Success : ActivityLogLevel.Error, "Network Tools", message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public void OpenSettings()
    {
        if (_networkService.OpenNetworkSettings())
        {
            ActivityLogService.Instance.Info("Network Tools", "Network settings opened.");
        }
    }
}
