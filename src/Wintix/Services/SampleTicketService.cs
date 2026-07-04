using Wintix.Models;

namespace Wintix.Services;

public sealed class SampleTicketService : ITicketService
{
    private static readonly IReadOnlyList<TicketItem> Tickets =
    [
        new TicketItem
        {
            Id = "tkt-001",
            EventTitle = "Neon Nights Festival",
            Venue = "Riverside Arena",
            EventDate = DateTimeOffset.Now.AddDays(14),
            Seat = "GA-1042",
            Status = "Valid",
            Barcode = "WTX-9F3A-7721"
        },
        new TicketItem
        {
            Id = "tkt-002",
            EventTitle = "Windows Dev Summit",
            Venue = "Convention Center Hall B",
            EventDate = DateTimeOffset.Now.AddDays(21),
            Seat = "VIP-018",
            Status = "Valid",
            Barcode = "WTX-2C11-9084"
        }
    ];

    public Task<IReadOnlyList<TicketItem>> GetMyTicketsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Tickets);
    }
}
