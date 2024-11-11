using Dapper;
using F1.Application.Database;
using F1.Application.Models;
using F1.Contracts.Requests;

namespace F1.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<bool> RegisterAsync(User user, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
                       insert into users (id, fullname, email, password, subscriptiontype)
                       values (@Id, @Fullname, @Email, @Password, @SubscriptionType)
                       """, user, cancellationToken: token));
        
        transaction.Commit();
        
        return result > 1;
    }

    public async Task<User> LoginAsync(LoginUserRequest request, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        var user = await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(
                @"select 
                                id,
                                fullName,
                                email,
                                password,
                                subscriptionType 
                         from users where email = @Email",
                new { Email = request.Email }, cancellationToken: token));
        if (user == null || user.Password != request.Password)
        {
            return null;
        }
        
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var user = await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition("""
                       select 
                           id,
                           fullName,
                           email,
                           password,
                           subscriptionType 
                       from users where id = @Id
                       """, new { id }, cancellationToken: token));

        if (user is null)
        {
            return null;
        }

        return user;
    }

    public async Task<User?> GetByEmailAsync(EmailUserRequest request, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var user = await connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition("""
                                  select
                                      id,
                                      fullName,
                                      email,
                                      password,
                                      subscriptionType
                                  from users where email = @Email
                                  """, new { request.Email }, cancellationToken: token));

        if (user is null)
        {
            return null;
        }

        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition("""
                                       select *
                                       from users
                                       """, cancellationToken: token));

        return result.Select(x => new User
        {
            Id = x.id,
            Email = x.email,
            FullName = x.fullname,
            Password = x.password,
            SubscriptionType = x.subscriptiontype
        });
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                        update users set fullname = @FullName, email = @Email, password = @Password,
                        subscriptiontype = @SubscriptionType
                        where id = @Id
                        """, user, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                        delete from users where id = @Id
                        """, new { id }, cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }
    
    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
               select count(1) from users where id = @id
               """, new { id }, cancellationToken: token));
    }
}