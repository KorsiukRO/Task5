using F1.Application.Services;
using F1.Contracts.Requests;
using F1.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Task5.Mapping;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Task5.Controllers;

[Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;

    public UsersController(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }
    
    private const string TokenSecret = "1S6XySoRS96vmuN=SQURaik^_rR.57^8)xTEL";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(911);
    
    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpPost(ApiEndpoints.Users.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request,
        CancellationToken token)
    {
        var user = request.MapToUser();
        await _userService.RegisterAsync(user, token);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }
    
    
    [AllowAnonymous]
    [HttpPost(ApiEndpoints.Users.Login)]
    public async Task<IActionResult> Register([FromBody] LoginUserRequest request,
        CancellationToken token)
    {
        var user = await _userService.LoginAsync(request, token);
        if (user is null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }
        
        
        var jwttoken = await _jwtTokenService.GenerateJwtTokenAsync(user);
        return Ok(jwttoken);
    }

    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpGet(ApiEndpoints.Users.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id,
        CancellationToken token)
    {
        var user = await _userService.GetByIdAsync(id, token);
        
        if (user is null)
        {
            return NotFound($"User with id '{id}' not found");
        }

        var response = user.MapToResponse();
        return Ok(response);
    }

    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpGet(ApiEndpoints.Users.GetByEmailUser)]
    public async Task<IActionResult> GetByEmailUser([FromBody] EmailUserRequest request,
        CancellationToken token)
    {
        var user = await _userService.GetByEmailAsync(request, token);
        
        if (user is null)
        {
            return NotFound($"User with email '{request.Email}' not found");
        }
    
        var response = user.MapToResponse();
        return Ok(response);
    }
    
    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpGet(ApiEndpoints.Users.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var users = await _userService.GetAllAsync(token);

        var usersResponse = users.MapToResponse();
        return Ok(usersResponse);
    }
    
    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpPut(ApiEndpoints.Users.Update)]
    public async Task<IActionResult> Update([FromRoute]Guid id,
        [FromBody]UpdateUserRequest request, CancellationToken token)
    {
        var user = request.MapToUser(id);
        var updatedRace = await _userService.UpdateAsync(request, user, token);
        if (updatedRace is null)
        {
            return NotFound();
        }

        var response = updatedRace.MapToResponse();
        return Ok(response);
    }
    
    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpDelete(ApiEndpoints.Users.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _userService.DeleteByIdAsync(id, token);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}