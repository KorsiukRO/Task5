using Dapper;
using F1.Application.Database;
using F1.Application.Models;
using F1.Contracts.Requests;
using Microsoft.Extensions.Configuration;

namespace F1.Application.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IConfiguration _config;

    public TicketRepository(IDbConnectionFactory dbConnectionFactory, IConfiguration config)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _config = config;
    }

    public async Task<bool> BuyTicketAsync(Ticket ticket, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
                        insert into tickets (ticket_id, user_id, race_id, date_event, price, ticket_type)
                        values (@TicketId, @UserId, @RaceId, @DateEvent, @Price, @TicketType)
                        """, ticket, cancellationToken: token));

        transaction.Commit();

        return result == 1;
    }

    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var ticket = await connection.QuerySingleOrDefaultAsync<Ticket>(
            new CommandDefinition("""
                                  select ticket_id AS TicketId, user_id AS UserId, race_id AS RaceId,
                                         date_event AS DateEvent, price AS Price, ticket_type AS TicketType
                                  from tickets
                                  where ticket_id = @Id
                                  """, new { id }, cancellationToken: token));

        if (ticket is null)
        {
            return null;
        }

        return ticket;
    }

    public async Task<IEnumerable<Ticket>> GetMyTicketsAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(new CommandDefinition("""
                                                                       select ticket_id, user_id, race_id,
                                                                       date_event, price, ticket_type
                                                                       from tickets
                                                                       where user_id = @Id
                                                                       """, new { id = id }, cancellationToken: token));

        return result.Select(x => new Ticket
        {
            TicketId = x.ticket_id,
            UserId = x.user_id,
            RaceId = x.race_id,
            DateEvent = x.date_event,
            Price = x.price,
            TicketType = x.ticket_type
        });
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByRaceAsync(GetTicketsByRaceRequest request,
        CancellationToken token)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var raceId = await connection.QuerySingleOrDefaultAsync<Guid?>(new CommandDefinition("""
            SELECT id
            FROM races
            WHERE namerace = @NameRace
            """, new { NameRace = request.NameRace }, cancellationToken: token));

        if (raceId == null)
        {
            return Enumerable.Empty<Ticket>();
        } 

        var result = await connection.QueryAsync<Ticket>(new CommandDefinition("""
                                                                               SELECT ticket_id AS TicketId, user_id AS UserId, race_id AS RaceId,
                                                                                      date_event AS DateEvent, price AS Price, ticket_type AS TicketType
                                                                               FROM tickets
                                                                               WHERE race_id = @RaceId
                                                                               """, new { RaceId = raceId },
            cancellationToken: token));

        return result.Select(x => new Ticket
        {
            TicketId = x.TicketId,
            UserId = x.UserId,
            RaceId = x.RaceId,
            DateEvent = x.DateEvent,
            Price = x.Price,
            TicketType = x.TicketType
        });
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByDateAsync(GetTicketsByDateRequest request,
        CancellationToken token)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var result = await connection.QueryAsync<Ticket?>(new CommandDefinition("""
                SELECT ticket_id AS TicketId, user_id AS UserId, race_id AS RaceId,
                date_event AS DateEvent, price AS Price, ticket_type AS TicketType
                FROM tickets
                WHERE date_event >= @StartDateEvent AND date_event <= @EndDateEvent
                """, new { StartDateEvent = request.StartDateEvent, EndDateEvent = request.EndDateEvent },
            cancellationToken: token));
        
        return result.Select(x => new Ticket
        {
            TicketId = x.TicketId,
            UserId = x.UserId,
            RaceId = x.RaceId,
            DateEvent = x.DateEvent,
            Price = x.Price,
            TicketType = x.TicketType
        });
    }

    public async Task<IEnumerable<Guid>> GetTopTicketsAsync(GetTopTicketsRequest request, CancellationToken token)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        var topRaceId = await connection.QueryAsync<Guid>(new CommandDefinition("""
                                        SELECT race_id
                                        FROM tickets
                                        GROUP BY race_id
                                        ORDER BY COUNT(*) DESC
                                        LIMIT @Top
                                        """, new { request.Top }, cancellationToken: token));
        
        return topRaceId;
    }
}
                