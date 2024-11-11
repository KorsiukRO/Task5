using F1.Contracts.Requests;
using F1.Application.Models;
using F1.Application.Repositories;
using FluentValidation;


namespace F1.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IValidator<BuyTicketRequest> _buyTicketValidator;
    private readonly IValidator<GetTicketsByRaceRequest> _getTicketByRaceValidator;
    private readonly IValidator<GetTicketsByDateRequest> _getTicketsByDateValidator;
    private readonly IValidator<GetTopTicketsRequest> _getTopTicketsValidator;

    public TicketService(ITicketRepository ticketRepository, 
        IValidator<BuyTicketRequest> buyTicketValidator,
        IValidator<GetTicketsByRaceRequest> getTicketByRaceValidator, 
        IValidator<GetTicketsByDateRequest> getTicketsByDateValidator, 
        IValidator<GetTopTicketsRequest> getTopTicketsValidator)
    {
        _ticketRepository = ticketRepository;
        _buyTicketValidator = buyTicketValidator;
        _getTicketByRaceValidator = getTicketByRaceValidator;
        _getTicketsByDateValidator = getTicketsByDateValidator;
        _getTopTicketsValidator = getTopTicketsValidator;
    }

    public async Task<bool> BuyTicketAsync(Ticket ticket, BuyTicketRequest request, CancellationToken token = default)
    {
        await _buyTicketValidator.ValidateAndThrowAsync(request, cancellationToken: token);
        return await _ticketRepository.BuyTicketAsync(ticket, token);
    }

    public Task<Ticket?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _ticketRepository.GetByIdAsync(id, token);
    }

    public Task<IEnumerable<Ticket>> GetMyTicketsAsync(Guid id, CancellationToken token = default)
    {
        return _ticketRepository.GetMyTicketsAsync(id, token);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByRaceAsync(GetTicketsByRaceRequest request, CancellationToken token)
    {
        await _getTicketByRaceValidator.ValidateAndThrowAsync(request, token);
        return await _ticketRepository.GetTicketsByRaceAsync(request, token);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByDateAsync(GetTicketsByDateRequest request, CancellationToken token)
    {
        await _getTicketsByDateValidator.ValidateAndThrowAsync(request, token);
        return await _ticketRepository.GetTicketsByDateAsync(request, token);
    }

    public async Task<IEnumerable<Guid>> GetTopTicketsAsync(GetTopTicketsRequest request, CancellationToken token)
    {
        await _getTopTicketsValidator.ValidateAndThrowAsync(request, token);
        return await _ticketRepository.GetTopTicketsAsync(request, token);
    }
}