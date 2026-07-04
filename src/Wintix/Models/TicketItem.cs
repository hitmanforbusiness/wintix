namespace Wintix.Models;

public sealed class TicketItem
{
    public required string Id { get; init; }
    public required string EventTitle { get; init; }
    public required string Venue { get; init; }
    public required DateTimeOffset EventDate { get; init; }
    public required string Seat { get; init; }
    public required string Status { get; init; }
    public required string Barcode { get; init; }

    public string FormattedDate => EventDate.ToString("f");
}
