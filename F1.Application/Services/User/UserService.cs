using F1.Application.Models;
using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User> _userValidator;
    private readonly IValidator<LoginUserRequest> _loginUserValidator;
    private readonly IValidator<EmailUserRequest> _emailUserValidator;
    private readonly IValidator<UpdateUserRequest> _updateUserValidator;

    public UserService(IUserRepository userRepository, 
        IValidator<User> userValidator, 
        IValidator<LoginUserRequest> loginUserValidator,
        IValidator<EmailUserRequest> emailUserValidator,
        IValidator<UpdateUserRequest> updateUserValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
        _loginUserValidator = loginUserValidator;
        _emailUserValidator = emailUserValidator;
        _updateUserValidator = updateUserValidator;
    }

    public async Task<bool> RegisterAsync(User user, CancellationToken token = default)
    {
        await _userValidator.ValidateAndThrowAsync(user, cancellationToken: token);
        return await _userRepository.RegisterAsync(user, token);
    }

    public async Task<User> LoginAsync(LoginUserRequest request, CancellationToken token = default)
    {
        await _loginUserValidator.ValidateAndThrowAsync(request, cancellationToken: token);
        return await _userRepository.LoginAsync(request, token);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _userRepository.GetByIdAsync(id, token);
    }
    
    public async Task<User?> GetByEmailAsync(EmailUserRequest request, CancellationToken token = default)
    {
        await _emailUserValidator.ValidateAndThrowAsync(request, cancellationToken: token);
        return await _userRepository.GetByEmailAsync(request, token);
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default)
    {
        return _userRepository.GetAllAsync(token);
    }

    public async Task<User?> UpdateAsync(UpdateUserRequest request, User user, CancellationToken token = default)
    {
        await _updateUserValidator.ValidateAndThrowAsync(request, cancellationToken: token);
        var raceExists = await _userRepository.ExistsByIdAsync(user.Id, token);
        if (!raceExists)
        {
            return null;
        }

        await _userRepository.UpdateAsync(user, token);
        return user;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _userRepository.DeleteByIdAsync(id, token);
    }
}