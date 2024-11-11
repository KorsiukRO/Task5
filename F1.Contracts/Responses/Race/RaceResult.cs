namespace F1.Contracts.Responses;

public class RaceResult
{
    public required string NameRace { get; set; }
    public required string Winner { get; set; }
    public required TimeSpan  Time { get; set; }
}