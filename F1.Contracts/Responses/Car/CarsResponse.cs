namespace F1.Contracts.Responses;

public class CarsResponse
{
    public required IEnumerable<CarResponse> Items { get; init; } = [];
}