using Wintix.Models;

namespace Wintix.Services;

public sealed class SampleEventService : IEventService
{
    private static readonly IReadOnlyList<EventItem> Events =
    [
        new EventItem
        {
            Id = "evt-001",
            Title = "Neon Nights Festival",
            Venue = "Riverside Arena",
            StartsAt = DateTimeOffset.Now.AddDays(14),
            Category = "Music",
            PriceFrom = 49.99m,
            IsFeatured = true
        },
        new EventItem
        {
            Id = "evt-002",
            Title = "Windows Dev Summit",
            Venue = "Convention Center Hall B",
            StartsAt = DateTimeOffset.Now.AddDays(21),
            Category = "Tech",
            PriceFrom = 0m,
            IsFeatured = true
        },
        new EventItem
        {
            Id = "evt-003",
            Title = "City Comedy Night",
            Venue = "The Laugh Loft",
            StartsAt = DateTimeOffset.Now.AddDays(7),
            Category = "Comedy",
            PriceFrom = 24.50m,
            IsFeatured = false
        },
        new EventItem
        {
            Id = "evt-004",
            Title = "Indie Film Premiere",
            Venue = "Metro Cinema",
            StartsAt = DateTimeOffset.Now.AddDays(30),
            Category = "Film",
            PriceFrom = 18.00m,
            IsFeatured = false
        }
    ];

    public Task<IReadOnlyList<EventItem>> GetFeaturedEventsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<EventItem>>(Events.Where(e => e.IsFeatured).ToList());
    }

    public Task<IReadOnlyList<EventItem>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Events);
    }
}
