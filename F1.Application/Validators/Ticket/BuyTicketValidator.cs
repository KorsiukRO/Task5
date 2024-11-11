using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class BuyTicketValidator : AbstractValidator<BuyTicketRequest>
{
    private readonly ITicketRepository _ticketRepository;

    public BuyTicketValidator(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;

        RuleFor(x => x.NameRace)
            .NotEmpty()
            .WithMessage("The \"NameRace\" can't be empty")
            .Matches(@"^[A-Za-z\s-]+$")
            .WithMessage("The \"NameRace\" must be the name of an F1 track (e.g., Silverstone, Monza, etc.)");
        RuleFor(x => x.TicketType)
            .NotEmpty()
            .WithMessage("The \"TicketType\" can't be empty")
            .Must(ticketType => ticketType == "basic" || ticketType == "premium")
            .WithMessage("The \"TicketType\" must be either \"basic\" or \"premium\".");
    }
    
    
}