using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class GetTicketsByDateValidator : AbstractValidator<GetTicketsByDateRequest>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketsByDateValidator(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
        
        RuleFor(x => x.StartDateEvent)
            .NotEmpty()
            .WithMessage("The \"StartDateEvent\" can't be empty");
        RuleFor(x => x.EndDateEvent)
            .NotEmpty()
            .WithMessage("The \"EndDateEvent\" can't be empty");
    }
}