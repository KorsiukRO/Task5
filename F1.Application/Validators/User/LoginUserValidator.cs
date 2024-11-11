using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    private readonly IUserRepository _userRepository;

    public LoginUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("The \"Email\" can't be empty")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
            .WithMessage("Invalid email format");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("The \"Password\" can't be empty");
    }
}