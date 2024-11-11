using F1.Application.Models;
using F1.Contracts.Requests;
using F1.Contracts.Responses;

namespace Task5.Mapping;

public static class ContractMappingTicket
{
    public static Ticket MapToTicket(this BuyTicketRequest request, string email, Race race, User user, IConfiguration config)
    {
        var userType = user.SubscriptionType.ToLower();
    
        var discountPercentage = userType switch
        {
            "fan" => int.Parse(config["Discounts:fan"]),
            "vip" => int.Parse(config["Discounts:vip"]),
            "all-inclusive" => int.Parse(config["Discounts:all-inclusive"]),
            _ => 0
        };
    
        var discountedPrice = race.BasicPrice - (race.BasicPrice * discountPercentage / 100);

        return new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = user.Id,
            RaceId = race.Id,
            DateEvent = race.DateEvent,
            Price = discountedPrice,
            TicketType = request.TicketType
        };
    }

    public static TicketResponse MapToResponse(this Ticket ticket)
    {
        return new TicketResponse()
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            RaceId = ticket.RaceId,
            DateEvent = ticket.DateEvent,
            Price = ticket.Price,
            TicketType = ticket.TicketType,
        };
    }
    
    public static TicketsResponse MapToResponse(this IEnumerable<Ticket> ticket)
    {
        return new TicketsResponse
        {
            Items = ticket.Select(MapToResponse)
        };
    }
}