using F1.Application.Models;

namespace F1.Application.Services;

public interface IJwtTokenService
{
    Task<String> GenerateJwtTokenAsync(User user, CancellationToken token = default);
}