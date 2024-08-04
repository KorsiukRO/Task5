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
                       insert into races (id, slug, namerace, passabilityrace)
                       values (@Id, @Slug, @NameRace, @PassabilityRace)
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

    public async Task<Race?> GetByNameAsync(StartRace race, CancellationToken token = default)
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
                        select id, namerace, passabilityrace
                        from races
                        """, cancellationToken: token));

        return result.Select(x => new Race
        {
            Id = x.id,
            NameRace = x.namerace,
            PassabilityRace = x.passabilityrace
        });
    }

    public async Task<bool> UpdateAsync(Race race, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                          update races set slug = @Slug, namerace = @NameRace, passabilityrace = @PassabilityRace
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