namespace F1.Contracts.Responses;

public class RaceResult
{
    public required string NameRace { get; set; }
    public required string Winner { get; set; }
    public required string Time { get; set; }
}