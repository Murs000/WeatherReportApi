using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business.Services.Implementations;
public class ReportService(IReportRepository reportRepository, IMapper mapper) : IReportService
{

    public async Task<IEnumerable<ReportDTO>> GetAllReportsAsync()
    {
        var reports = await reportRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ReportDTO>>(reports);
    }

    public async Task<ReportDTO> GetReportByIdAsync(int id)
    {
        var report = await reportRepository.GetByIdAsync(id);
        return mapper.Map<ReportDTO>(report);
    }

    public async Task<ReportDTO> CreateReportAsync(ReportDTO reportDTO)
    {
        var report = mapper.Map<Report>(reportDTO);
        report.SetCredentials();
        await reportRepository.AddAsync(report);
        return mapper.Map<ReportDTO>(report);
    }

    public async Task<ReportDTO> UpdateReportAsync(ReportDTO reportDTO)
    {
        var report = mapper.Map<Report>(reportDTO);
        report.SetCredentials();
        await reportRepository.UpdateAsync(report);
        return mapper.Map<ReportDTO>(report);
    }

    public async Task<bool> DeleteReportAsync(int id)
    {
        return await reportRepository.DeleteAsync(id);
    }
}