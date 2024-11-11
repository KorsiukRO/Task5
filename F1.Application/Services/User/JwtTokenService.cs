using F1.Application.Models;
using F1.Application.Repositories;

namespace F1.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IJwtTokenRepository _jwtTokenRepository;

    public JwtTokenService(IJwtTokenRepository jwtTokenRepository)
    {
        _jwtTokenRepository = jwtTokenRepository;
    }

    public Task<string> GenerateJwtTokenAsync(User user, CancellationToken token = default)
    {
        return _jwtTokenRepository.GenerateJwtTokenAsync(user, token);
    }
}