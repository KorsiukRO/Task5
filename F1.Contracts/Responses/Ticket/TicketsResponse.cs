namespace F1.Contracts.Responses;

public class TicketsResponse
{
    public required IEnumerable<TicketResponse> Items { get; init; } = [];
}