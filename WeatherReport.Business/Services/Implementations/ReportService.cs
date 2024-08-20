using AutoMapper;
using WeatherReport.Business.DTOs;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Repositories.Interfaces;

namespace WeatherReport.Business.Services.Implementations;
public class ReportService(IRepositoryUnitOfWork repository, IMapper mapper) : IReportService
{

    public async Task<IEnumerable<ReportDTO>> GetAllAsync()
    {
        var reports = await repository.ReportRepository.GetAllAsync();
        return mapper.Map<IEnumerable<ReportDTO>>(reports);
    }

    public async Task<ReportDTO> GetByIdAsync(int id)
    {
        var report = await repository.ReportRepository.GetByIdAsync(id);
        return mapper.Map<ReportDTO>(report);
    }

    public async Task<ReportDTO> AddAsync(ReportDTO reportDTO)
    {
        var report = mapper.Map<Report>(reportDTO);
        report.SetCredentials();
        await repository.ReportRepository.AddAsync(report);
        return mapper.Map<ReportDTO>(report);
    }

    public async Task<ReportDTO> UpdateAsync(ReportDTO reportDTO)
    {
        var report = mapper.Map<Report>(reportDTO);
        report.SetCredentials();
        await repository.ReportRepository.UpdateAsync(report);
        return mapper.Map<ReportDTO>(report);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.ReportRepository.DeleteAsync(id);
    }
}