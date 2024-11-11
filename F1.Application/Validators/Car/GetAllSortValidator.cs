using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class GetAllSortValidator : AbstractValidator<GetAllSortCarRequest>
{
    private readonly ICarRepository _carRepository;

    public GetAllSortValidator(ICarRepository carRepository)
    {
        _carRepository = carRepository;
        RuleFor(x => x.SortBySpeed)
            .NotEmpty()
            .WithMessage("The \"SortBySpeed\" can't be empty")
            .Must(value => value == "Asc" || value == "Desc")
            .WithMessage("The \"SortBySpeed\" must be either 'Asc' or 'Desc'");
    }
}