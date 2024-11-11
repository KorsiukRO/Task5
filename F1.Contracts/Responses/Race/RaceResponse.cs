namespace F1.Contracts.Responses;

public class RaceResponse
{
    public required Guid Id { get; init; }
    
    public required string Slug { get; init; }
    
    public required string NameRace { get; init; }
    
    public required int PassabilityRace { get; init; }
    
    public required string Location { get; init; }
    
    public required DateTime DateEvent { get; init; }
    
    public required int BasicPrice { get; init; }
    
    public required string SubscriptionType { get; init; }
}