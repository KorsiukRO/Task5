using F1.Application.Models;
using F1.Application.Repositories;
using FluentValidation;

namespace F1.Application.Validators;

public class CarValidator : AbstractValidator<Car>
{
    private readonly ICarRepository _carRepository;
    
    public CarValidator(ICarRepository carRepository)
    {
        _carRepository = carRepository;
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The Id can't be empty");
        
        RuleFor(x => x.Speed)
            .NotEmpty()
            .WithMessage("The Speed can't be empty");
        
        RuleFor(x => x.Passability)
            .NotEmpty()
            .WithMessage("The Passability can't be empty");
        
        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .WithMessage("The Manufacturer can't be empty");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("The Model can't be empty");

        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This car already exists in the system");
    }

    private async Task<bool> ValidateSlug(Car car, string slug, CancellationToken token = default)
    {
        var existingCar = await _carRepository.GetBySlugAsync(slug);

        if (existingCar is not null)
        {
            return existingCar.Id == car.Id;
        }

        return existingCar is null;
    }
}