using F1.Application.Models;

namespace F1.Application.Repositories;

public interface IJwtTokenRepository
{
    Task<String> GenerateJwtTokenAsync(User user, CancellationToken token = default);
}