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
    private readonly IValidator<NameRaceRequest> _nameRaceValidator;
    private readonly IValidator<GetAllInRangeRequest> _getAllInRangeValidator;
    
    public RaceService(IRaceRepository raceRepository, IValidator<Race> raceValidator, 
        IValidator<NameRaceRequest> nameRaceValidator, IValidator<GetAllInRangeRequest> getAllInRangeValidator)
    {
        _raceRepository = raceRepository;
        _raceValidator = raceValidator;
        _nameRaceValidator = nameRaceValidator;
        _getAllInRangeValidator = getAllInRangeValidator;
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

    
    public async Task<Race?> GetByNameAsync(NameRaceRequest race, CancellationToken token = default)
    {
        await _nameRaceValidator.ValidateAndThrowAsync(race, cancellationToken: token);
        return await _raceRepository.GetByNameAsync(race, token);
    }

    public Task<IEnumerable<Race>> GetAllAsync(CancellationToken token = default)
    {
        return _raceRepository.GetAllAsync(token);
    }

    public Task<IEnumerable<Race>> GetAllSortByDateEvent(CancellationToken token = default)
    {
        return _raceRepository.GetAllSortByDateEvent(token);
    }

    public async Task<IEnumerable<Race>> GetAllInRangeAsync(GetAllInRangeRequest request, CancellationToken token = default)
    {
        await _getAllInRangeValidator.ValidateAndThrowAsync(request, cancellationToken: token);
        return await _raceRepository.GetAllInRangeAsync(request, token);
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
        var timeSpan = new TimeSpan(random.Next(1, 3), random.Next(0, 60), random.Next(0, 60));

        return new RaceResult
        {
            NameRace = race.NameRace, 
            Winner = $"{winningCar?.Manufacturer} {winningCar?.Model}" ?? "Unknown",
            Time = timeSpan
        };
    }
}