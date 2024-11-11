using System.Security.Claims;
using F1.Application.Services;
using F1.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task5.Mapping;

namespace Task5.Controllers;

[Authorize]
[ApiController]
public class TicketsController :ControllerBase
{
    private ITicketService _ticketService;
    private IUserService _userService;
    private IRaceService _raceService;
    private readonly IConfiguration _configuration;

    public TicketsController(ITicketService ticketService, IUserService userService, IRaceService raceService, IConfiguration configuration)
    {
        _ticketService = ticketService;
        _userService = userService;
        _raceService = raceService;
        _configuration = configuration;
    }

    
    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpPost(ApiEndpoints.Tickets.Buy)]
    public async Task<IActionResult> BuyTiсket([FromBody] BuyTicketRequest request, CancellationToken token)
    {
        var nameRaceRequest = new NameRaceRequest
        {
            NameRace = request.NameRace
        };
        
        var race = await _raceService.GetByNameAsync(nameRaceRequest, token);
        if (race == null) { return NotFound("No such race was found."); }

        var email = HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) { return Unauthorized("The user is not authorized or the email is missing from the token."); }

        var emailUserRequest = new EmailUserRequest { Email = email };
        
        var user = await _userService.GetByEmailAsync(emailUserRequest);
        if (user == null) { return Unauthorized("The user is not authorized or the user is Not Found."); }
        
        var ticket = request.MapToTicket(email, race, user, _configuration);
        
        await _ticketService.BuyTicketAsync(ticket, request, token);
        
        return  CreatedAtAction(nameof(Get), new { id = ticket.TicketId }, ticket);
    }
    
    
    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Tickets.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id,
        CancellationToken token)
    {
        var ticket = await _ticketService.GetByIdAsync(id, token);
        if (ticket is null) { return NotFound(); }

        var response = ticket.MapToResponse();
        return Ok(response);
    }

    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Tickets.GetMyTickets)]
    public async Task<IActionResult> GetMyTickets(CancellationToken token)
    {
        var email = HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null) return Unauthorized("The user is not authorized or the email is missing from the token.");

        var emailUserRequest = new EmailUserRequest { Email = email };
        
        var user = await _userService.GetByEmailAsync(emailUserRequest); 
        if (user == null) return NotFound("The user is not found.");

        var tickets = await _ticketService.GetMyTicketsAsync(user.Id);
        var result = tickets.MapToResponse();
        return Ok(result);
    }

    [Authorize(Policy = "VipOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Tickets.GetTicketsByRace)]
    public async Task<IActionResult> GetTicketsByRace([FromBody] GetTicketsByRaceRequest request, CancellationToken token)
    {
        var tickets = await _ticketService.GetTicketsByRaceAsync(request, token);
        if (tickets is null) { return NotFound(); }
        var result = tickets.MapToResponse();
        return Ok(result);
    }

    [Authorize(Policy = "VipOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Tickets.GetTicketsByDate)]
    public async Task<IActionResult> GetTicketsByDate([FromBody] GetTicketsByDateRequest request,
        CancellationToken token)
    {
        var tickets = await _ticketService.GetTicketsByDateAsync(request, token);
        var result = tickets.MapToResponse();
        return Ok(result);
    }

    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Tickets.GetTopTickets)]
    public async Task<IActionResult> GetTopTickets([FromBody] GetTopTicketsRequest request,
        CancellationToken token)
    {
        var topRacesId = await _ticketService.GetTopTicketsAsync(request, token);

        var raceTasks = topRacesId.Select(raceId => _raceService.GetByIdAsync(raceId, token));
        var topraces = await Task.WhenAll(raceTasks);
        var result = topraces.MapToResponseTop();
        return Ok(result);
    }
}