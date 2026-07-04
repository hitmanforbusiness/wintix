using Wintix.Models;

namespace Wintix.Services;

public interface IEventService
{
    Task<IReadOnlyList<EventItem>> GetFeaturedEventsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EventItem>> GetUpcomingEventsAsync(CancellationToken cancellationToken = default);
}
