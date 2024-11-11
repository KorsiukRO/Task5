using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class GetAllInRangeValidator : AbstractValidator<GetAllInRangeRequest>
{
    private readonly IRaceRepository _raceRepository;

    public GetAllInRangeValidator(IRaceRepository raceRepository)
    {
        _raceRepository = raceRepository;

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("The \"StartDate\" can't be empty");
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("The \"EndDate\" can't be empty");
    }
}