using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<ReportDTO>> GetAllAsync();
    Task<ReportDTO> GetByIdAsync(int id);
    Task<ReportDTO> AddAsync(ReportDTO reportDTO);
    Task<ReportDTO> UpdateAsync(ReportDTO reportDTO);
    Task<bool> DeleteAsync(int id);
}
