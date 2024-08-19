using Microsoft.EntityFrameworkCore;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.DataAccess;

public class WeatherReportDb : DbContext
{
    public WeatherReportDb(DbContextOptions<WeatherReportDb> options) : base(options) {}
    public DbSet<Report> Report => Set<Report>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();
    public DbSet<Forecast> Forecasts => Set<Forecast>();

    // Configuring entity relationships and properties
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Eagerly load all related entities
        modelBuilder.Entity<Report>()
                .Navigation(r => r.Forecasts)
                .AutoInclude();  // Auto-include forecasts with reports

        modelBuilder.Entity<Subscriber>()
                .Navigation(s => s.Reports)
                .AutoInclude();  // Auto-include reports with subscribers

        // Subscriber configuration
        modelBuilder.Entity<Subscriber>()
            .HasMany(s => s.Reports)
            .WithOne(r => r.Subscriber)
            .HasForeignKey(r => r.SubscriberId);

        // Report configuration
        modelBuilder.Entity<Report>()
            .HasMany(r => r.Forecasts)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // Set SubscriptionType enum to be stored as a string
        modelBuilder.Entity<Subscriber>()
            .Property(s => s.SubscriptionType)
            .HasConversion<string>();

        // Apply global query filters for soft delete
        modelBuilder.Entity<Subscriber>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<Report>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<Forecast>().HasQueryFilter(f => !f.IsDeleted);
    }
}