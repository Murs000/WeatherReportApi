using System.ComponentModel.DataAnnotations;

namespace WeatherReport.DataAccess.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModifyDate { get; set; }
    public bool IsDeleted { get; set; }

    public void SetCredentials()
    {
        if (CreationDate == default)
        {
            CreationDate = DateTime.UtcNow;
        }
        ModifyDate = DateTime.UtcNow;
    }
}