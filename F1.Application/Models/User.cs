namespace F1.Application.Models;

public partial class User
{
    public required Guid Id { get; init; }

    public required string FullName { get; init; }
    
    public required string Email { get; init; }
    
    public required string Password { get; init; }
    
    public required string SubscriptionType { get; init; }
}