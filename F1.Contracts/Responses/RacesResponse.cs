namespace F1.Contracts.Responses;

public class RacesResponse
{
    public required IEnumerable<RaceResponse> Items { get; init; } = Enumerable.Empty<RaceResponse>();
}