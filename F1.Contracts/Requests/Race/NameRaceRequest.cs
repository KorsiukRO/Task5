namespace F1.Contracts.Requests;

public record NameRaceRequest
{
    public required string NameRace { get; init; }
}