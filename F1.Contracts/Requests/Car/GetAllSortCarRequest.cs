namespace F1.Contracts.Requests;

public record GetAllSortCarRequest
{
    public required string SortBySpeed { get; init; }
}