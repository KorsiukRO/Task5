using F1.Application.Models;
using F1.Application.Repositories;
using FluentValidation;

namespace F1.Application.Validators;

public class RaceValidator : AbstractValidator<Race>
{
    private readonly IRaceRepository _raceRepository;
    
    public RaceValidator(IRaceRepository raceRepository)
    {
        _raceRepository = raceRepository;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The Id can't be empty");
        RuleFor(x => x.NameRace)
            .NotEmpty()
            .WithMessage("The name race can't be empty")
            .Matches(@"^[A-Za-z\s-]+$")
            .WithMessage("The \"NameRace\" must be the name of an F1 track (e.g., Silverstone, Monza, etc.)");
        RuleFor(x => x.PassabilityRace)
            .NotEmpty()
            .WithMessage("The passability race can't be empty")
            .InclusiveBetween(0, 100)
            .WithMessage("The passability race must be between 0 and 100.");
        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This race already exists in the system");
    }
    
    private async Task<bool> ValidateSlug(Race race, string slug, CancellationToken token = default)
    {
        var existingCar = await _raceRepository.GetBySlugAsync(slug);

        if (existingCar is not null)
        {
            return existingCar.Id == race.Id;
        }

        return existingCar is null;
    }
}