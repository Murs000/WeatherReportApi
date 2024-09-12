using WeatherReport.API.Middlewares;
using WeatherReport.Business;
using WeatherReport.DataAccess;
using WeatherReport.API;
using WeatherReport.Business.Services.Interfaces;
using WeatherReport.Business.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAppSettings(builder.Configuration);

builder.Services.AddAppMappers();

builder.Services.AddAppDB(builder.Configuration);

builder.Services.AddHttpClient<IWeatherApiService, WeatherApiService>();

builder.Services.AddQuartzJobs();

builder.Services.AddAppRepositories();

builder.Services.AddAppServices();

var app = builder.Build();

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