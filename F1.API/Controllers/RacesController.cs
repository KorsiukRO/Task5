using F1.Application.Services;
using F1.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task5.Mapping;

namespace Task5.Controllers;

[Authorize]
[ApiController]
public class RacesController : ControllerBase
{
    private IRaceService _raceService;
    private ICarService _carService;

    public RacesController(IRaceService raceService, ICarService carService)
    {
        _raceService = raceService;
        _carService = carService;
    }
    
    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpPost(ApiEndpoints.Races.Create)]
    public async Task<IActionResult> Create([FromBody] CreateRaceRequest request,
        CancellationToken token)
    {
        var race = request.MapToRace();
        await _raceService.CreateAsync(race, token);
        return CreatedAtAction(nameof(Get), new { idOrSlug = race.Id }, race);
    }
    
    
    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Races.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var race = Guid.TryParse(idOrSlug, out var id)
            ? await _raceService.GetByIdAsync(id, token)
            : await _raceService.GetBySlugAsync(idOrSlug, token);
        
        if (race is null)
        {
            return NotFound();
        }

        var response = race.MapToResponse();
        return Ok(response);
    }

    
    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Races.GetByNameRace)] 
    public async Task<IActionResult> GetByNameRace([FromBody]NameRaceRequest request, CancellationToken token)
    {
        var races = await _raceService.GetByNameAsync(request, token);
        var racesResponse = races.MapToResponse();
        return Ok(racesResponse);
    }
    
    
    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Races.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var races = await _raceService.GetAllAsync(token);

        var racesResponse = races.MapToResponse();
        return Ok(racesResponse);
    }


    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Races.GetAllSortByDateEvent)]
    public async Task<IActionResult> GetAllSortByDateEvent(CancellationToken token)
    {
        var races = await _raceService.GetAllSortByDateEvent(token);
        var racesResponse = races.MapToResponse();
        return Ok(racesResponse);
    }


    [Authorize(Policy = "FanOrVIPOrAllInclusiveUsers")]
    [HttpGet(ApiEndpoints.Races.GetAllInRange)]
    public async Task<IActionResult> GetAllInRange([FromBody]GetAllInRangeRequest request, CancellationToken token)
    {
        var races = await _raceService.GetAllInRangeAsync(request, token);
        var racesResponse = races.MapToResponse();
        return Ok(racesResponse);
    }


    [Authorize(Policy = "AllInclusiveUser")]
    [HttpPut(ApiEndpoints.Races.Update)]
    public async Task<IActionResult> Update([FromRoute]Guid id,
        [FromBody]UpdateRaceRequest request, CancellationToken token)
    {
        var race = request.MapToRace(id);
        var updatedRace = await _raceService.UpdateAsync(race, token);
        if (updatedRace is null)
        {
            return NotFound();
        }

        var response = updatedRace.MapToResponse();
        return Ok(response);
    }

    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpDelete(ApiEndpoints.Races.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _raceService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }

    
    [Authorize(Policy = "AllInclusiveUser")]
    [HttpPost(ApiEndpoints.Races.Start)]
    public async Task<IActionResult> StartRace([FromBody] NameRaceRequest request,
        CancellationToken token)
    {
        var getRace = await _raceService.GetByNameAsync(request, token);
        if (getRace is null)
        {
            return NotFound("Race does not exist.");
        }

        var allCars = await _carService.GetAllAsync(token);
        var listCars = allCars.ToList();
        if (!listCars.Any())
        {
            return NotFound("No cars available for the race.");
        }
        
        var raceResult = await _raceService.RunRace(listCars, getRace);
        var response = raceResult.MapToResponse();
        return Ok(response);
    }
}
