using F1.Application.Models;
using F1.Contracts.Requests;
using F1.Contracts.Responses;

namespace Task5.Mapping;

public static class ContractMappingRace
{
    public static Race MapToRace(this CreateRaceRequest request)
    {
        return new Race
        {
            Id = Guid.NewGuid(),
            NameRace = request.NameRace,
            PassabilityRace = request.PassabilityRace,
            Location = request.Location,
            DateEvent = request.DateEvent,
            BasicPrice = request.BasicPrice,
            SubscriptionType = request.SubscriptionType
        };
    }

    public static Race MapToRace(this UpdateRaceRequest request, 
        Guid id)
    {
        return new Race
        {
            Id = id,
            NameRace = request.NameRace,
            PassabilityRace = request.PassabilityRace,
            Location = request.Location,
            DateEvent = request.DateEvent,
            BasicPrice = request.BasicPrice,
            SubscriptionType = request.SubscriptionType
        };
    }
    
    public static RaceResponse MapToResponse(this Race race)
    {
        return new RaceResponse()
        {
            Id = race.Id,
            Slug = race.Slug,
            NameRace = race.NameRace,
            PassabilityRace = race.PassabilityRace,
            Location = race.Location,
            DateEvent = race.DateEvent,
            BasicPrice = race.BasicPrice,
            SubscriptionType = race.SubscriptionType
        };
    }

    public static RaceResult MapToResponse(this RaceResult race)
    {
        return new RaceResult()
        {
            NameRace = race.NameRace,
            Winner = race.Winner,
            Time = race.Time
        };
    }
    
    public static RacesResponse MapToResponse(this IEnumerable<Race> races)
    {
        return new RacesResponse
        {
            Items = races.Select(MapToResponse)    
        };
        
    }
    
    public static RaceResponseTop MapToResponseTop(this Race race)
    {
        return new RaceResponseTop()
        {
            NameRace = race.NameRace,
            PassabilityRace = race.PassabilityRace,
            Location = race.Location,
            DateEvent = race.DateEvent,
            BasicPrice = race.BasicPrice
        };
    }
    
    public static RacesResponseTop MapToResponseTop(this IEnumerable<Race> races)
    {
        return new RacesResponseTop
        {
            Items = races.Select(race => race.MapToResponseTop()).ToList()
        };
    }
}