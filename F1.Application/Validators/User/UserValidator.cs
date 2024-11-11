using F1.Application.Models;
using F1.Application.Repositories;
using FluentValidation;
using FluentValidation.Validators;

namespace F1.Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    private readonly IUserRepository _userRepository;
    
    public UserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The \"Id\" can't be empty");
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