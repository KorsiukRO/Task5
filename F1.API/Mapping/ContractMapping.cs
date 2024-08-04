using F1.Application.Models;
using F1.Contracts.Requests;
using F1.Contracts.Responses;

namespace Task5.Mapping;

public static class ContractMapping
{
    public static Car MapToCar(this CreateCarRequest request)
    {
        return new Car
        {
            Id = Guid.NewGuid(),
            Speed = request.Speed,
            Passability = request.Passability,
            Manufacturer = request.Manufacturer,
            Model = request.Model
        };
    }
    
    public static Car MapToCar(this UpdateCarRequest request, Guid id)
    {
        return new Car
        {
            Id = id,
            Speed = request.Speed,
            Passability = request.Passability,
            Manufacturer = request.Manufacturer,
            Model = request.Model
        };
    }
    
    public static Race MapToRace(this CreateRaceRequest request)
    {
        return new Race
        {
            Id = Guid.NewGuid(),
            NameRace = request.NameRace,
            PassabilityRace = request.PassabilityRace
        };
    }
    
    public static Race MapToRace(this UpdateRaceRequest request, 
        Guid id)
    {
        return new Race
        {
            Id = id,
            NameRace = request.NameRace,
            PassabilityRace = request.PassabilityRace
        };
    }

    public static CarResponse MapToResponse(this Car car)
    {
        return new CarResponse()
        {
            Id = car.Id,
            Manufacturer = car.Manufacturer,
            Model = car.Model,
            Passability = car.Passability,
            Speed = car.Speed,
            Slug = car.Slug
        };
    }

    public static RaceResponse MapToResponse(this Race race)
    {
        return new RaceResponse()
        {
            Id = race.Id,
            Slug = race.Slug,
            NameRace = race.NameRace,
            PassabilityRace = race.PassabilityRace
        };
    }

    public static RaceResultResponse MapToResponse(this RaceResult race)
    {
        return new RaceResultResponse()
        {
            NameRace = race.NameRace,
            Winner = race.Winner,
            Time = race.Time
        };
    }

    public static CarsResponse MapToResponse(this IEnumerable<Car> cars)
    {
        return new CarsResponse
        {
            Items = cars.Select(MapToResponse)
        };
    }

    public static RacesResponse MapToResponse(this IEnumerable<Race> races)
    {
        return new RacesResponse
        {
            Items = races.Select(MapToResponse)    
        };
        
    }
}