namespace F1.Contracts.Requests;

public class UpdateRaceRequest
{
    public required string NameRace { get; init; }
    
    public required int PassabilityRace { get; init; }
}