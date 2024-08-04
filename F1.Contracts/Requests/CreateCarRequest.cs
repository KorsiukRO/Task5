namespace F1.Contracts.Requests;

public class CreateCarRequest
{
    public required int Speed { get; init; }
    
    public required int Passability { get; init; }
    
    public required string Manufacturer { get; init; } 
    
    public required string Model { get; init; }
}