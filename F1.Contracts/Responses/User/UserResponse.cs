namespace F1.Contracts.Responses;

public class UserResponse
{
    public required Guid Id { get; init; }
    
    public required string FullName { get; init; }
    
    public required string Email { get; init; }
    
    public required string SubscriptionType { get; init; }
}