namespace Wintix.Models;

public sealed class EventItem
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Venue { get; init; }
    public required DateTimeOffset StartsAt { get; init; }
    public required string Category { get; init; }
    public decimal PriceFrom { get; init; }
    public bool IsFeatured { get; init; }

    public string FormattedDate => StartsAt.ToString("f");

    public string FormattedPrice => PriceFrom <= 0 ? "Free" : $"From ${PriceFrom:0.00}";
}
