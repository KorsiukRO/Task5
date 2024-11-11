namespace F1.Contracts.Responses;

public class RacesResponseTop
{
    public required IEnumerable<RaceResponseTop> Items { get; init; } = [];
}