using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class EventsViewModel : ObservableObject
{
    private readonly IEventService _eventService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    public IReadOnlyList<EventItem> Events { get; private set; } = [];

    public EventsViewModel()
        : this(new SampleEventService())
    {
    }

    public EventsViewModel(IEventService eventService)
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
            Events = await _eventService.GetUpcomingEventsAsync();
            OnPropertyChanged(nameof(Events));
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
