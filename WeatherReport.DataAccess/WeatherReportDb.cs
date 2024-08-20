using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.DataAccess;

public class WeatherReportDb : DbContext
{
    private readonly IConfiguration _configuration;
    public WeatherReportDb(DbContextOptions<WeatherReportDb> options, IConfiguration configuration) 
        : base(options)
    {
        _configuration = configuration;
    }
    public DbSet<Report> Report => Set<Report>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();
    public DbSet<WeatherDetail> WeatherDetails => Set<WeatherDetail>();
    public DbSet<Forecast> Forecasts => Set<Forecast>();

    // Configuring entity relationships and properties
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Retrieve admin user details from appsettings.json
        var adminUser = _configuration.GetSection("AdminUser").Get<Subscriber>();

        // Create admin user from configuration
        modelBuilder.Entity<Subscriber>().HasData(
            new Subscriber
            {
                Id = 1, // Ensure the Id is set to match your primary key constraints
                Name = adminUser.Name,
                Surname = adminUser.Surname,
                Email = adminUser.Email,
                CityOfResidence = adminUser.CityOfResidence,
                SubscriptionType = adminUser.SubscriptionType,
                SubscriptionDate = DateTime.Now.ToUniversalTime(),
                CreationDate = DateTime.Now.ToUniversalTime(),
                ModifyDate = DateTime.Now.ToUniversalTime(),
                IsDeleted = false
            }
        );
        
        // Eagerly load all related entities
        modelBuilder.Entity<Report>()
                .Navigation(r => r.WeatherDetails)
                .AutoInclude();  // Auto-include weatherDetails with reports

        modelBuilder.Entity<Subscriber>()
                .Navigation(s => s.Forecasts)
                .AutoInclude();  // Auto-include reports with subscribers

        modelBuilder.Entity<Forecast>()
                .Navigation(s => s.Reports)
                .AutoInclude();  // Auto-include reports with forecasts

        // Subscriber configuration
        modelBuilder.Entity<Subscriber>()
            .HasMany(s => s.Forecasts)
            .WithOne(r => r.Subscriber)
            .HasForeignKey(r => r.SubscriberId);

        // Report configuration
        modelBuilder.Entity<Report>()
            .HasMany(r => r.WeatherDetails)
            .WithOne(w => w.Report)
            .HasForeignKey(w=> w.ReportId);

        // Subscriber configuration
        modelBuilder.Entity<Forecast>()
            .HasMany(s => s.Reports)
            .WithOne(r => r.Forecast)
            .HasForeignKey(r => r.ForecastId);

        // Set SubscriptionType enum to be stored as a string
        modelBuilder.Entity<Subscriber>()
            .Property(s => s.SubscriptionType)
            .HasConversion<string>();

        // Apply global query filters for soft delete
        modelBuilder.Entity<Subscriber>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<Report>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<Forecast>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<WeatherDetail>().HasQueryFilter(f => !f.IsDeleted);
    }
}