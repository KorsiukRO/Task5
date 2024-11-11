namespace F1.Contracts.Requests;

public record GetTopTicketsRequest()
{
    public required int Top { get; init; } = 3;
}