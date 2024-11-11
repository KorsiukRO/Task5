namespace F1.Contracts.Requests;

public record GetTicketsByRaceRequest
{
    public required string NameRace { get; init; }
}