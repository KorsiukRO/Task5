namespace F1.Contracts.Requests;

public record BuyTicketRequest
{
    public required string NameRace { get; init; }
    
    public required string TicketType { get; init; }
}
