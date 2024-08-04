using F1.Application.Models;

namespace F1.Application.Services;

public interface ICarService
{
    Task<bool> CreateAsync(Car car, CancellationToken token = default);
    
    Task<Car?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<Car?> GetBySlugAsync(string slug, CancellationToken token = default);
    
    Task<IEnumerable<Car>> GetAllAsync(CancellationToken token = default);
    
    Task<Car?> UpdateAsync(Car car, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}