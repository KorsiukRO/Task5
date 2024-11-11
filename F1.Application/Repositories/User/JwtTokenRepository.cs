using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using F1.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace F1.Application.Repositories;

public class JwtTokenRepository : IJwtTokenRepository
{
    private readonly IConfiguration _config;
    
    public JwtTokenRepository(IConfiguration config)
    {
        _config = config;
    }
    public Task<string> GenerateJwtTokenAsync(User user, CancellationToken token = default)
    {
        var secretKey = _config["Jwt:Key"];
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException(nameof(secretKey), "JWT Secret key is not configured.");
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("SubscriptionType", user.SubscriptionType)
            }),
            Expires = DateTime.UtcNow.AddHours(10),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenJwt = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(tokenJwt));
    }
}