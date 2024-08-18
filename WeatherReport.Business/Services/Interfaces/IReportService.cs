using WeatherReport.Business.DTOs;

namespace WeatherReport.Business.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<ReportDTO>> GetAllReportsAsync();
    Task<ReportDTO> GetReportByIdAsync(int id);
    Task<ReportDTO> CreateReportAsync(ReportDTO reportDTO);
    Task<ReportDTO> UpdateReportAsync(ReportDTO reportDTO);
    Task<bool> DeleteReportAsync(int id);
}
