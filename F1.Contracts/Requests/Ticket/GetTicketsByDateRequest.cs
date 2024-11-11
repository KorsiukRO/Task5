namespace F1.Contracts.Requests;

public record GetTicketsByDateRequest
{
    public required DateTime StartDateEvent { get; init; }
    
    public required DateTime EndDateEvent { get; init; }
}