using F1.Application.Models;
using F1.Contracts.Requests;

namespace F1.Application.Repositories;

public interface IRaceRepository
{
    Task<bool> CreateAsync(Race race, CancellationToken token = default);
    
    Task<Race?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<Race?> GetBySlugAsync(string slug, CancellationToken token = default);
    
    Task<Race?> GetByNameAsync(NameRaceRequest race, CancellationToken token = default);
    
    Task<IEnumerable<Race>> GetAllAsync(CancellationToken token = default);
    
    Task<IEnumerable<Race>> GetAllSortByDateEvent(CancellationToken token = default);

    Task<IEnumerable<Race>> GetAllInRangeAsync(GetAllInRangeRequest request, CancellationToken token = default);
    
    Task<bool> UpdateAsync(Race race, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}