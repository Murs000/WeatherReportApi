using Microsoft.EntityFrameworkCore;
using Quartz;
using WeatherReport.API.Configurations;
using WeatherReport.API.Middlewares;
using WeatherReport.Business.Jobs;
using WeatherReport.Business.MappingProfiles;
using WeatherReport.Business.Services.Implementations;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Settings;
using WeatherReport.DataAccess;
using WeatherReport.DataAccess.Repositories.Implementations;
using WeatherReport.DataAccess.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(SubscriberProfile));
builder.Services.AddAutoMapper(typeof(ReportProfile));
builder.Services.AddAutoMapper(typeof(WeatherDetailProfile));
builder.Services.AddAutoMapper(typeof(ForecastProfile));

// Register PostgreSQL database context
builder.Services.AddDbContext<WeatherReportDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<WeatherApiSettings>(builder.Configuration.GetSection("ExternalApi"));

// Register External API service with HttpClient
builder.Services.AddHttpClient<IWeatherApiService, WeatherApiService>();

// Register Email settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register Email service
builder.Services.AddScoped<IEmailService, EmailService>();

// Load QuartzSettings from appsettings.json
builder.Services.Configure<QuartzSettings>(builder.Configuration.GetSection("QuartzSettings"));

// Register Quartz.NET services
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Register background jobs
builder.Services.AddScoped<DailyEmailJob>();
builder.Services.AddScoped<HourlyReportJob>();

// Register the repositories and services
builder.Services.AddScoped<IRepositoryUnitOfWork, RepositoryUnitOfWork>();

builder.Services.AddScoped<IServiceUnitOfWork, ServiceUnitOfWork>();

builder.Services.AddScoped<IJobService, JobService>();

var app = builder.Build();

QuartzJobScheduler.ConfigureJobs(builder.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WeatherReportDb>();
    db.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();