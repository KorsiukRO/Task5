using System.Text.RegularExpressions;
using F1.Contracts.Responses;

namespace F1.Application.Models;

public partial class Ticket
{
    public required Guid TicketId { get; init; }

    public required Guid UserId { get; init; }
    
    public required Guid RaceId { get; init; }
    
    public required DateTime DateEvent { get; init; }
    
    public required int Price { get; init; }
    
    public required string TicketType { get; init; }
}