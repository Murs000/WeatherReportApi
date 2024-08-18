using Microsoft.AspNetCore.Mvc;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;

namespace WeatherReport.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReportController (IReportService reportService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReportDTO>>> GetReports()
    {
        var reports = await reportService.GetAllReportsAsync();
        return Ok(reports);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReportDTO>> GetReport(int id)
    {
        var report = await reportService.GetReportByIdAsync(id);
        if (report == null)
        {
            return NotFound();
        }
        return Ok(report);
    }

    [HttpPost]
    public async Task<ActionResult<ReportDTO>> CreateReport(ReportDTO reportDTO)
    {
        var createdReport = await reportService.CreateReportAsync(reportDTO);
        return CreatedAtAction(nameof(GetReport), new { id = createdReport.Id }, createdReport);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReport(int id, ReportDTO reportDTO)
    {
        if (id != reportDTO.Id)
        {
            return BadRequest();
        }

        var updatedReport = await reportService.UpdateReportAsync(reportDTO);
        if (updatedReport == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReport(int id)
    {
        var result = await reportService.DeleteReportAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}