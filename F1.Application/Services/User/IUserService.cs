using F1.Application.Models;
using F1.Contracts.Requests;

namespace F1.Application.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(User user, CancellationToken token = default);
    
    Task<User> LoginAsync(LoginUserRequest request, CancellationToken token = default);
    
    Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<User?> GetByEmailAsync(EmailUserRequest request, CancellationToken token = default);
    
    Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);
    
    Task<User?> UpdateAsync(UpdateUserRequest request, User user, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}