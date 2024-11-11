namespace F1.Contracts.Requests;

public record FullNameUserRequest
{
    public required string FullName { get; init; }
}