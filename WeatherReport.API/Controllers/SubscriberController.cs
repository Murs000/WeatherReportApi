using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriberController(IServiceUnitOfWork service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriberDTO>>> GetAll()
    {
        var subscriberDTOs = await service.SubscriberService.GetAllAsync();
        return Ok(subscriberDTOs);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<SubscriberDTO>> GetById(int id)
    {
        var subscriberDTO = await service.SubscriberService.GetByIdAsync(id);
        if (subscriberDTO == null)
        {
            return NotFound();
        }
        return Ok(subscriberDTO);
    }
    [HttpPost]
    public async Task<ActionResult> Add([FromBody]SubscriberDTO subscriberDTO)
    {
        await service.SubscriberService.AddAsync(subscriberDTO);
        return CreatedAtAction(nameof(GetById), new { id = subscriberDTO.Id }, subscriberDTO);
    }
    [HttpPut]
    public async Task<ActionResult> Update([FromBody]SubscriberDTO subscriberDTO)
    {
        if (service.SubscriberService.GetByIdAsync(subscriberDTO.Id) == null)
        {
            return BadRequest();
        }

        await service.SubscriberService.UpdateAsync(subscriberDTO);
        return Ok(subscriberDTO);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if(await service.SubscriberService.DeleteAsync(id))
        {
            return Ok();
        }
        return NotFound();
    }
}