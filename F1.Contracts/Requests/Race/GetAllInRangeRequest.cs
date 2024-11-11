namespace F1.Contracts.Requests;

public record GetAllInRangeRequest
{
    public required DateTime StartDate { get; init;}
    public required DateTime EndDate { get; init;}
}