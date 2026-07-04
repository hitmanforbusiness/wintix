using Wintix.Models;

namespace Wintix.Services;

public interface ITicketService
{
    Task<IReadOnlyList<TicketItem>> GetMyTicketsAsync(CancellationToken cancellationToken = default);
}
