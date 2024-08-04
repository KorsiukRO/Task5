namespace F1.Contracts.Responses;

public class CarResponse
{
    public required Guid Id { get; init; }
    
    public required string Slug { get; init; }
    
    public required int Speed { get; init; }
    
    public required int Passability { get; init; }
    
    public required string Manufacturer { get; init; } 
    
    public required string Model { get; init; }
}