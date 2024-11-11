using Dapper;
using F1.Application.Database;
using F1.Application.Models;
using F1.Contracts.Requests;

namespace F1.Application.Repositories;

public class RaceRepository : IRaceRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RaceRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Race race, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                       insert into races (id, slug, namerace, passabilityrace, location, 
                                          dateevent, basicprice, subscriptiontype)
                       values (@Id, @Slug, @NameRace, @PassabilityRace, @Location, 
                       @Dateevent, @Basicprice, @Subscriptiontype)
                       """, race, cancellationToken: token));
        
        transaction.Commit();
        
        return result > 1;
    }
    
    public async Task<Race?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var race = await connection.QuerySingleOrDefaultAsync<Race>(
            new CommandDefinition("""
            select * from races where id = @Id
            """, new { id }, cancellationToken: token));
        
        if (race is null)
        {
            return null;
        }
        
        return race;
    }
    
    public async Task<Race?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var race = await connection.QuerySingleOrDefaultAsync<Race>(
            new CommandDefinition("""
                                  select * from races where slug = @Slug
                                  """, new { slug }, cancellationToken: token));
        
        if (race is null)
        {
            return null;
        }
        
        return race;
    }
    
    public async Task<Race?> GetByNameAsync(NameRaceRequest race, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var foundRace = await connection.QuerySingleOrDefaultAsync<Race>(new CommandDefinition("""
            select * from races where namerace = @nameRace
            """, new { race.NameRace }, cancellationToken: token));
        
        return foundRace;
    }
    
    public async Task<IEnumerable<Race>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition("""
                        select * 
                        from races
                        """, cancellationToken: token));
        
        return result.Select(x => new Race
        {
            Id = x.id,
            NameRace = x.namerace,
            PassabilityRace = x.passabilityrace,
            Location = x.location,
            DateEvent = x.dateevent,
            BasicPrice = x.basicprice,
            SubscriptionType = x.subscriptiontype
        });
    }

    public async Task<IEnumerable<Race>> GetAllSortByDateEvent(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition($"""
                                       select *
                                       from races
                                       ORDER BY dateevent ASC;
                                       """, cancellationToken: token));
        
        return result.Select(x => new Race
        {
            Id = x.id,
            NameRace = x.namerace,
            PassabilityRace = x.passabilityrace,
            Location = x.location,
            DateEvent = x.dateevent,
            BasicPrice = x.basicprice,
            SubscriptionType = x.subscriptiontype
        });
    }

    public async Task<IEnumerable<Race>> GetAllInRangeAsync(GetAllInRangeRequest request, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition(
                                       @"SELECT *
                                       FROM races
                                       WHERE dateevent BETWEEN @StartDate AND @EndDate
                                       ORDER BY dateevent ASC",
                                       new { request.StartDate, request.EndDate }, 
                                       cancellationToken: token));
        
        return result.Select(x => new Race
        {
            Id = x.id,
            NameRace = x.namerace,
            PassabilityRace = x.passabilityrace,
            Location = x.location,
            DateEvent = x.dateevent,
            BasicPrice = x.basicprice,
            SubscriptionType = x.subscriptiontype
        });
    }

    public async Task<bool> UpdateAsync(Race race, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                          update races set slug = @Slug, namerace = @NameRace, passabilityrace = @PassabilityRace, 
                                           location = @Location, dateevent = @DateEvent, basicprice = @BasicPrice, 
                                           subscriptiontype = @SubscriptionType  
                          where id = @Id
                          """, race, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                          delete from races where id = @Id
                          """, new { id }, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }
    
    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                  select count(1) from races where id = @id
                  """, new { id }, cancellationToken: token));
    }
}