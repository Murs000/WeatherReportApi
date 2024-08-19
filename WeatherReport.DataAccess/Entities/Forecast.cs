using System.ComponentModel.DataAnnotations;

namespace WeatherReport.DataAccess.Entities;

public class Forecast : BaseEntity
{
    public string Description { get; set; }
    public string Icon { get; set; }
}