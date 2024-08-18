using Microsoft.EntityFrameworkCore;
using WeatherReport.DataAccess.Entities;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.DataAccess;

public class WeatherReportDb : DbContext
{
    public WeatherReportDb(DbContextOptions<WeatherReportDb> options) : base(options) {}
    public DbSet<Report> Report => Set<Report>();
    public DbSet<Subscriber> Subscribers => Set<Subscriber>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Subscriber>()
            .Property(s => s.SubscriptionType)
            .HasConversion<string>();  // Converts the enum to string in the database
    }
}