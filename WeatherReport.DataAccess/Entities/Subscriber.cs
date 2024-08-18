using System.ComponentModel.DataAnnotations;
using WeatherReport.DataAccess.Enums;

namespace WeatherReport.DataAccess.Entities;

public class Subscriber
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string CityOfResidence { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    //TODO: relation,Subcriptiondate
    public DateTime CreationDate { get; set; }
    public bool IsDeleted { get; set; }

    public void SetCredentials()
    {
        CreationDate = DateTime.Now.ToUniversalTime();
    }
}