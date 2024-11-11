namespace F1.Contracts.Requests;

public record EmailUserRequest
{
    public required string Email { get; init; }
}