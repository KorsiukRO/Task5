using Dapper;
using F1.Application.Database;
using F1.Application.Models;

namespace F1.Application.Repositories;

public class CarRepository : ICarRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public CarRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<bool> CreateAsync(Car car, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
                          insert into cars (id, slug, speed, passability, manufacturer, model)
                          values (@Id, @Slug, @Speed, @Passability, @Manufacturer, @Model)
                          """, car, cancellationToken: token));
        
        transaction.Commit();
        
        return result > 1;
    }

    public async Task<Car?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var car = await connection.QuerySingleOrDefaultAsync<Car>(
            new CommandDefinition("""
            select * from cars where id = @Id
            """, new { id }, cancellationToken: token));

        if (car is null)
        {
            return null;
        }

        return car;
    }

    public async Task<Car?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var car = await connection.QuerySingleOrDefaultAsync<Car>(
            new CommandDefinition("""
                                  select * from cars where slug = @Slug
                                  """, new { slug }, cancellationToken: token));

        if (car is null)
        {
            return null;
        }

        return car;
    }

    public async Task<IEnumerable<Car>> GetAllAsync(CancellationToken token = default)
    {
    using var connection = await _dbConnectionFactory.CreateConnectionAsync();
    var result = await connection.QueryAsync(new CommandDefinition("""
        select id, speed, passability, manufacturer, model
        from cars
        """, cancellationToken: token));
    
    return result.Select(x => new Car
    {
        Id = x.id,
        Speed = x.speed,
        Passability = x.passability,
        Manufacturer = x.manufacturer,
        Model = x.model
    });
}

    public async Task<bool> UpdateAsync(Car car, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
             update cars set slug = @Slug, speed = @Speed, passability = @Passability, 
             manufacturer = @Manufacturer, model = @Model
             where id = @Id
             """, car, cancellationToken: token));
        
        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
             delete from cars where id = @Id
             """, new { id }, cancellationToken: token));
                                                                         
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
             select count(1) from cars where id = @id
             """, new { id }, cancellationToken: token));
    } 
}