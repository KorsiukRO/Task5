using F1.Application.Models;
using F1.Application.Repositories;
using F1.Contracts.Requests;
using F1.Contracts.Responses;
using FluentValidation;

namespace F1.Application.Services;

public class RaceService : IRaceService
{
    private readonly IRaceRepository _raceRepository;
    private readonly IValidator<Race> _raceValidator;
    
    public RaceService(IRaceRepository raceRepository, IValidator<Race> raceValidator)
    {
        _raceRepository = raceRepository;
        _raceValidator = raceValidator;
    }

    public async Task<bool> CreateAsync(Race race, CancellationToken token = default)
    {
        await _raceValidator.ValidateAndThrowAsync(race, cancellationToken: token);
        return await _raceRepository.CreateAsync(race, token);
    }

    public Task<Race?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _raceRepository.GetByIdAsync(id, token);
    }

    public Task<Race?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        return _raceRepository.GetBySlugAsync(slug, token);
    }

    public Task<Race?> GetByNameAsync(StartRace race, CancellationToken token = default)
    {
        return _raceRepository.GetByNameAsync(race, token);
    }

    public Task<IEnumerable<Race>> GetAllAsync(CancellationToken token = default)
    {
        return _raceRepository.GetAllAsync(token);
    }

    public async Task<Race?> UpdateAsync(Race race, CancellationToken token = default)
    {
        await _raceValidator.ValidateAndThrowAsync(race, cancellationToken: token);
        var raceExists = await _raceRepository.ExistsByIdAsync(race.Id, token);
        if (!raceExists)
        {
            return null;
        }

        await _raceRepository.UpdateAsync(race, token);
        return race;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _raceRepository.DeleteByIdAsync(id, token);
    }

    public async Task<RaceResult> RunRace(IEnumerable<Car> cars, Race race)
    {
        var random = new Random();
        var winningCar = cars.OrderBy(c => random.Next()).FirstOrDefault();
        var time = $"{random.Next(1, 3)}:{random.Next(0, 59):00}:{random.Next(0, 59):00}";

        return new RaceResult
        {
            NameRace = race.NameRace, 
            Winner = $"{winningCar?.Manufacturer} {winningCar?.Model}" ?? "Unknown",
            Time = time
        };
    }
}