using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReportController (IServiceUnitOfWork service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReportDTO>>> GetAll()
    {
        var reportDTOs = await service.ReportService.GetAllAsync();
        return Ok(reportDTOs);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ReportDTO>> GetById(int id)
    {
        var reportDTO = await service.ReportService.GetByIdAsync(id);
        if (reportDTO == null)
        {
            return NotFound();
        }
        return Ok(reportDTO);
    }
    [HttpPost]
    public async Task<ActionResult> Add([FromBody]ReportDTO reportDTO)
    {
        await service.ReportService.AddAsync(reportDTO);
        return CreatedAtAction(nameof(GetById), new { id = reportDTO.Id }, reportDTO);
    }
    [HttpPut]
    public async Task<ActionResult> Update([FromBody]ReportDTO reportDTO)
    {
        if (service.ReportService.GetByIdAsync(reportDTO.Id) == null)
        {
            return BadRequest();
        }

        await service.ReportService.UpdateAsync(reportDTO);
        return Ok(reportDTO);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if(await service.ReportService.DeleteAsync(id))
        {
            return Ok();
        }
        return NotFound();
    }
}