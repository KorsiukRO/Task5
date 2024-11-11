using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class GetTopTicketsValidator : AbstractValidator<GetTopTicketsRequest>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTopTicketsValidator(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
        
        RuleFor(x => x.Top)
            .NotEmpty()
            .WithMessage("The \"Top\" can't be empty")
            .InclusiveBetween(1, 30)
            .WithMessage("The \"Top\" must be between 1 and 30.");
    }
}