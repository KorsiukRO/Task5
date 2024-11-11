namespace F1.Contracts.Responses;

public class TicketResponse
{
    public required Guid TicketId { get; init; }

    public required Guid UserId { get; init; }
    
    public required Guid RaceId { get; init; }
    
    public required DateTime DateEvent { get; init; }
    
    public required int Price { get; init; }
    
    public required string TicketType { get; init; }
}