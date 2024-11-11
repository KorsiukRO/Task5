using F1.Application.Models;
using F1.Contracts.Requests;
using F1.Contracts.Responses;

namespace Task5.Mapping;

public static class ContractMappingCar
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

    public static CarsResponse MapToResponse(this IEnumerable<Car> cars)
    {
        return new CarsResponse
        {
            Items = cars.Select(MapToResponse)
        };
    }
}