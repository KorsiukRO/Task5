using F1.Application.Models;
using F1.Contracts.Requests;
using F1.Contracts.Responses;

namespace F1.Application.Repositories;

public interface ITicketRepository
{
    Task<bool> BuyTicketAsync(Ticket ticket, CancellationToken token = default);
    
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<IEnumerable<Ticket>> GetMyTicketsAsync(Guid id, CancellationToken token = default);
    
    Task<IEnumerable<Ticket>> GetTicketsByRaceAsync(GetTicketsByRaceRequest request, CancellationToken token);
    
    Task<IEnumerable<Ticket>> GetTicketsByDateAsync(GetTicketsByDateRequest request, CancellationToken token);
    
    Task<IEnumerable<Guid>> GetTopTicketsAsync(GetTopTicketsRequest request, CancellationToken token);
}