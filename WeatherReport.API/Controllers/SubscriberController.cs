using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriberController : ControllerBase
{
    private readonly ISubscriberService _subscriberService;

    public SubscriberController(ISubscriberService subscriberService)
    {
        _subscriberService = subscriberService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubscriberDTO>> GetById(int id)
    {
        var subscriber = await _subscriberService.GetByIdAsync(id);
        if (subscriber == null)
        {
            return NotFound();
        }
        return Ok(subscriber);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriberDTO>>> GetAll()
    {
        var subscribers = await _subscriberService.GetAllAsync();
        return Ok(subscribers);
    }

    [HttpPost]
    public async Task<ActionResult> Add(SubscriberDTO subscriberDto)
    {
        await _subscriberService.AddAsync(subscriberDto);
        return CreatedAtAction(nameof(GetById), new { id = subscriberDto.Id }, subscriberDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SubscriberDTO subscriberDto)
    {
        if (id != subscriberDto.Id)
        {
            return BadRequest();
        }

        await _subscriberService.UpdateAsync(subscriberDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _subscriberService.DeleteAsync(id);
        return NoContent();
    }
}