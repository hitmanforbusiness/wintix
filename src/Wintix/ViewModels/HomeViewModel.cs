using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class HomeViewModel : ObservableObject
{
    private readonly IEventService _eventService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    public IReadOnlyList<EventItem> FeaturedEvents { get; private set; } = [];

    public HomeViewModel()
        : this(new SampleEventService())
    {
    }

    public HomeViewModel(IEventService eventService)
    {
        _eventService = eventService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;
        ErrorMessage = null;

        try
        {
            FeaturedEvents = await _eventService.GetFeaturedEventsAsync();
            OnPropertyChanged(nameof(FeaturedEvents));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
