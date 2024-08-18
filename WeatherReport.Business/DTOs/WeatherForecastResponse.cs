using System.Text.Json.Serialization;

namespace WeatherReport.Business.DTOs
{
    public class WeatherForecastResponse
    {
        [JsonPropertyName("cod")]
        public string Cod { get; set; }
        
        [JsonPropertyName("message")]
        public int Message { get; set; }
        
        [JsonPropertyName("cnt")]
        public int Cnt { get; set; }
        
        [JsonPropertyName("list")]
        public List<WeatherForecast> List { get; set; }
        
        [JsonPropertyName("city")]
        public CityInfo City { get; set; }
    }

    public class WeatherForecast
    {
        [JsonPropertyName("dt")]
        public int Dt { get; set; }
        
        [JsonPropertyName("main")]
        public MainInfo Main { get; set; }
        
        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; }
        
        [JsonPropertyName("clouds")]
        public CloudInfo CloudInfo { get; set; } // Renamed from "Clouds"
        
        [JsonPropertyName("wind")]
        public WindInfo WindInfo { get; set; } // Renamed from "Wind"
        
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }
        
        [JsonPropertyName("pop")]
        public double Pop { get; set; } // Probability of precipitation
        
        [JsonPropertyName("rain")]
        public Rain Rain { get; set; }
        
        [JsonPropertyName("snow")]
        public Snow Snow { get; set; }
        
        [JsonPropertyName("sys")]
        public SysInfo Sys { get; set; }
        
        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; } // Time of data forecasted in ISO format
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; } // Temperature
        
        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; } // Feels like temperature
        
        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; } // Minimum temperature
        
        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; } // Maximum temperature
        
        [JsonPropertyName("pressure")]
        public int Pressure { get; set; } // Atmospheric pressure
        
        [JsonPropertyName("sea_level")]
        public int SeaLevel { get; set; } // Atmospheric pressure on sea level
        
        [JsonPropertyName("grnd_level")]
        public int GrndLevel { get; set; } // Atmospheric pressure on ground level
        
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; } // Humidity
        
        [JsonPropertyName("temp_kf")]
        public double TempKf { get; set; } // Internal parameter
    }

    public class WeatherInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } // Weather condition id
        
        [JsonPropertyName("main")]
        public string Main { get; set; } // Group of weather parameters
        
        [JsonPropertyName("description")]
        public string Description { get; set; } // Weather condition description
        
        [JsonPropertyName("icon")]
        public string Icon { get; set; } // Weather icon id
    }

    public class CloudInfo
    {
        [JsonPropertyName("all")]
        public int All { get; set; } // Cloudiness
    }

    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; } // Wind speed
        
        [JsonPropertyName("deg")]
        public int Deg { get; set; } // Wind direction in degrees
        
        [JsonPropertyName("gust")]
        public double Gust { get; set; } // Wind gust speed
    }

    public class Rain
    {
        [JsonPropertyName("3h")]
        public double? _3h { get; set; } // Rain volume for last 3 hours (nullable)
    }

    public class Snow
    {
        [JsonPropertyName("3h")]
        public double? _3h { get; set; } // Snow volume for last 3 hours (nullable)
    }

    public class SysInfo
    {
        [JsonPropertyName("pod")]
        public string Pod { get; set; } // Part of the day (n - night, d - day)
    }

    public class CityInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } // City ID
        
        [JsonPropertyName("name")]
        public string Name { get; set; } // City name
        
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; } // City coordinates
        
        [JsonPropertyName("country")]
        public string Country { get; set; } // Country code
        
        [JsonPropertyName("population")]
        public int Population { get; set; } // City population
        
        [JsonPropertyName("timezone")]
        public int Timezone { get; set; } // Shift in seconds from UTC
        
        [JsonPropertyName("sunrise")]
        public int Sunrise { get; set; } // Sunrise time
        
        [JsonPropertyName("sunset")]
        public int Sunset { get; set; } // Sunset time
    }

    public class Coord
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; } // Latitude
        
        [JsonPropertyName("lon")]
        public double Lon { get; set; } // Longitude
    }
}