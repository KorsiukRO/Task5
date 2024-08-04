using Dapper;

namespace F1.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
            create table if not exists cars (
            id UUID primary key,
            slug TEXT not null,
            speed integer not null,
            passability integer not null,
            manufacturer Text not null,
            model Text not null);
         """);

        await connection.ExecuteAsync("""
            create unique index concurrently if not exists cars_slug_idx
            on cars
            using btree(slug);
        """);
        
        await connection.ExecuteAsync("""
            create table if not exists races (
            id UUID primary key,
            slug TEXT not null,
            NameRace Text not null,
            PassabilityRace integer not null);
         """);

        await connection.ExecuteAsync("""
            create unique index concurrently if not exists races_slug_idx
            on races
            using btree(slug);
        """);
    }
}