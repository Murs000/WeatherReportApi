using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers.v1;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin")]
public class WeatherDetailController (IServiceUnitOfWork service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherDetailDTO>>> GetAll()
    {
        var weatherDetailDTOs = await service.WeatherDetailService.GetAllAsync();
        return Ok(weatherDetailDTOs);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<WeatherDetailDTO>> GetById(int id)
    {
        var weatherDetailDTO = await service.WeatherDetailService.GetByIdAsync(id);
        if (weatherDetailDTO == null)
        {
            return NotFound();
        }
        return Ok(weatherDetailDTO);
    }
    [HttpPost]
    public async Task<ActionResult> Add([FromBody]WeatherDetailDTO weatherDetailDTO)
    {
        await service.WeatherDetailService.AddAsync(weatherDetailDTO);
        return CreatedAtAction(nameof(GetById), new { id = weatherDetailDTO.Id }, weatherDetailDTO);
    }
    [HttpPut]
    public async Task<ActionResult> Update([FromBody]WeatherDetailDTO weatherDetailDTO)
    {
        if (service.WeatherDetailService.GetByIdAsync(weatherDetailDTO.Id) == null)
        {
            return BadRequest();
        }

        await service.WeatherDetailService.UpdateAsync(weatherDetailDTO);
        return Ok(weatherDetailDTO);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if(await service.WeatherDetailService.DeleteAsync(id))
        {
            return Ok();
        }
        return NotFound();
    }
}