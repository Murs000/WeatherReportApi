using System.ComponentModel.DataAnnotations;

namespace WeatherReport.DataAccess.Entities;

public class Report
{
    [Key]
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    // TODO: Modify date,base entity
    public bool IsDeleted { get; set; }

    public void SetCredentials()
    {
        CreationDate = DateTime.Now.ToUniversalTime();
    }
}
