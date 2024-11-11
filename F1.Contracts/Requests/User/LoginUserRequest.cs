namespace F1.Contracts.Requests;


public record LoginUserRequest
{
    public required string Email { get; init; }
    
    public required string Password { get; init; }
}