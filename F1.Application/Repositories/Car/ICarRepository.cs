using F1.Application.Models;
using F1.Contracts.Requests;

namespace F1.Application.Repositories;

public interface ICarRepository
{
   Task<bool> CreateAsync(Car car, CancellationToken token = default);
    
    Task<Car?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<Car?> GetBySlugAsync(string slug, CancellationToken token = default);
    
    Task<IEnumerable<Car>> GetAllAsync(CancellationToken token = default);
    
    Task<IEnumerable<Car>> GetAllSortAsync(GetAllSortCarRequest request, CancellationToken token = default);
    
    Task<bool> UpdateAsync(Car car, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}