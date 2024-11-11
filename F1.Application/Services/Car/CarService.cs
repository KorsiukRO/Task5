using F1.Application.Models;
using F1.Application.Repositories;
using F1.Contracts.Requests;
using FluentValidation;

namespace F1.Application.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IValidator<Car> _carValidator;
    private readonly IValidator<GetAllSortCarRequest> _getAllSortCarValidator;

    public CarService(ICarRepository carRepository, IValidator<Car> carValidator, IValidator<GetAllSortCarRequest> getAllSortCarValidator)
    {
        _carRepository = carRepository;
        _carValidator = carValidator;
        _getAllSortCarValidator = getAllSortCarValidator;
    }

    public async Task<bool> CreateAsync(Car car, CancellationToken token = default)
    {
        await _carValidator.ValidateAndThrowAsync(car, cancellationToken: token);
        return await _carRepository.CreateAsync(car, token);
    }

    public Task<Car?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _carRepository.GetByIdAsync(id, token);
    }

    public Task<Car?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        return _carRepository.GetBySlugAsync(slug, token);
    }

    public Task<IEnumerable<Car>> GetAllAsync(CancellationToken token = default)
    {
        return _carRepository.GetAllAsync(token);
    }
    
    public async Task<IEnumerable<Car>> GetAllSortAsync(GetAllSortCarRequest request, CancellationToken token = default)
    {
        await _getAllSortCarValidator.ValidateAndThrowAsync(request, cancellationToken: token);
        return await _carRepository.GetAllSortAsync(request, token);
    }

    public async Task<Car?> UpdateAsync(Car car, CancellationToken token = default)
    {
        await _carValidator.ValidateAndThrowAsync(car, cancellationToken: token);
        var carExists = await _carRepository.ExistsByIdAsync(car.Id, token);
        if (!carExists)
        {
            return null;
        }

        await _carRepository.UpdateAsync(car, token);
        return car;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _carRepository.DeleteByIdAsync(id, token);
    }
}