using F1.Application.Services;
using F1.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using Task5.Mapping;

namespace Task5.Controllers;

[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpPost(ApiEndpoints.Cars.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCarRequest request,
        CancellationToken token)
    {
        var car = request.MapToCar();
        await _carService.CreateAsync(car, token);
        return CreatedAtAction(nameof(Get), new { idOrSlug = car.Id }, car);
    }

    [HttpGet(ApiEndpoints.Cars.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var car = Guid.TryParse(idOrSlug, out var id)
            ? await _carService.GetByIdAsync(id, token)
            : await _carService.GetBySlugAsync(idOrSlug, token);
        if (car is null)
        {
            return NotFound();
        }

        return Ok(car.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Cars.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var cars = await _carService.GetAllAsync(token);
        
        var carsResponse = cars.MapToResponse();
        return Ok(carsResponse);
    }

    [HttpPut(ApiEndpoints.Cars.Update)]
    public async Task<IActionResult> Update([FromRoute]Guid id,
        [FromBody]UpdateCarRequest request,
        CancellationToken token)
    {
        var car = request.MapToCar(id);
        var updatedCar = await _carService.UpdateAsync(car, token);
        if (updatedCar is null)
        {
            return NotFound();
        }

        var response = updatedCar.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Cars.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id,
        CancellationToken token)
    {
        var deleted = await _carService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
