namespace F1.Contracts.Responses;

public class RaceResponse
{
    public required Guid Id { get; init; }
    
    public required string Slug { get; init; }
    
    public required string NameRace { get; init; }
    
    public required int PassabilityRace { get; init; }
}