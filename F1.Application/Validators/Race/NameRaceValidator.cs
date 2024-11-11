using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class NameRaceValidator : AbstractValidator<NameRaceRequest>
{
    private readonly IRaceRepository _raceRepository;
    
    public NameRaceValidator(IRaceRepository raceRepository)
    {
        _raceRepository = raceRepository;
        
        RuleFor(x => x.NameRace)
            .NotEmpty()
            .WithMessage("The \"NameRace\" can't be empty")
            .Matches(@"^[A-Za-z\s-]+$")
            .WithMessage("The \"NameRace\" must be the name of an F1 track (e.g., Silverstone, Monza, etc.)");
    }
}