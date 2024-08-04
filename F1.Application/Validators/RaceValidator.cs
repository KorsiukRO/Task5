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
            .WithMessage("The Speed can't be empty");
        RuleFor(x => x.NameRace)
            .NotEmpty()
            .WithMessage("The name race can't be empty");
        RuleFor(x => x.PassabilityRace)
            .NotEmpty()
            .WithMessage("The passability race can't be empty");
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