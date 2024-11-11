using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("The \"Fullname\" can't be empty");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("The \"Email\" can't be empty")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
            .WithMessage("Invalid email format");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("The \"Password\" can't be empty")
            .MinimumLength(10)
            .WithMessage("Password must be at least 10 characters long");
        RuleFor(x => x.SubscriptionType)
            .NotEmpty()
            .WithMessage("The \"Subscription type\" can't be empty")
            .Must(type => type == "fan" || type == "vip" || type == "all-inclusive")
            .WithMessage("Subscription type must be 'fan', 'vip', or 'all-inclusive'");
    }
}