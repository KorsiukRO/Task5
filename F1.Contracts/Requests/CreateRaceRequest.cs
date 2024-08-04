namespace F1.Contracts.Requests;

public class CreateRaceRequest
{
    public required string NameRace { get; init; }
    
    public required int PassabilityRace { get; init; }
}