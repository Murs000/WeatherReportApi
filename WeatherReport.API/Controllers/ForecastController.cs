using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ForecastController (IServiceUnitOfWork service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForecastDTO>>> GetAll()
    {
        var forecastDTOs = await service.ForecastService.GetAllAsync();
        return Ok(forecastDTOs);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ForecastDTO>> GetById(int id)
    {
        var forecastDTO = await service.ForecastService.GetByIdAsync(id);
        if (forecastDTO == null)
        {
            return NotFound();
        }
        return Ok(forecastDTO);
    }
    [HttpPost]
    public async Task<ActionResult> Add([FromBody]ForecastDTO forecastDTO)
    {
        await service.ForecastService.AddAsync(forecastDTO);
        return CreatedAtAction(nameof(GetById), new { id = forecastDTO.Id }, forecastDTO);
    }
    [HttpPut]
    public async Task<ActionResult> Update([FromBody]ForecastDTO forecastDTO)
    {
        if (service.ForecastService.GetByIdAsync(forecastDTO.Id) == null)
        {
            return BadRequest();
        }

        await service.ForecastService.UpdateAsync(forecastDTO);
        return Ok(forecastDTO);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if(await service.ForecastService.DeleteAsync(id))
        {
            return Ok();
        }
        return NotFound();
    }
}