using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Validators;

public class EmailUserValidator : AbstractValidator<EmailUserRequest>
{
    private readonly IUserRepository _userRepository;

    public EmailUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("The \"Email\" can't be empty")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$")
            .WithMessage("Invalid email format");
    }
}