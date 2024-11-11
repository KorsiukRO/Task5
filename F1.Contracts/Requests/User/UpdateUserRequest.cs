namespace F1.Contracts.Requests;

public record UpdateUserRequest
{
    public required string FullName { get; init; }
    
    public required string Email { get; init; }
    
    public required string Password { get; init; }
    
    public required string SubscriptionType { get; init; }
}