using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wintix.Models;
using Wintix.Services;

namespace Wintix.ViewModels;

public sealed partial class TicketsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    public IReadOnlyList<TicketItem> Tickets { get; private set; } = [];

    public TicketsViewModel()
        : this(new SampleTicketService())
    {
    }

    public TicketsViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
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
            Tickets = await _ticketService.GetMyTicketsAsync();
            OnPropertyChanged(nameof(Tickets));
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
