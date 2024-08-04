using F1.Application.Models;
using F1.Contracts.Requests;
using F1.Contracts.Responses;

namespace F1.Application.Services;

public interface IRaceService
{
    Task<bool> CreateAsync(Race race, CancellationToken token = default);
    
    Task<Race?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<Race?> GetBySlugAsync(string slug, CancellationToken token = default);
    
    Task<Race?> GetByNameAsync(StartRace race, CancellationToken token = default);
    
    Task<IEnumerable<Race>> GetAllAsync(CancellationToken token = default);
    
    Task<Race?> UpdateAsync(Race race, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<RaceResult> RunRace(IEnumerable<Car> cars, Race race);
}